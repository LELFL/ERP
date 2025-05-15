using Interfaces;
using StackExchange.Redis;

namespace Services;

internal class IdentityService : IIdentityService
{
    private const string IDENTITY_PERMISSIONS_KEY = "IDENTITY_PERMISSIONS_KEY_";
    private const string LOCK_SUFFIX = "_LOCK";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan LockTimeout = TimeSpan.FromSeconds(5);
    private static readonly int MaxRetryCount = 3;

    private readonly IDatabase _redis;
    private readonly IIdentityRemoteService _identityRemoteService;

    public IdentityService(IConnectionMultiplexer connectionMultiplexer, IIdentityRemoteService identityRemoteService)
    {
        _redis = connectionMultiplexer.GetDatabase();
        _identityRemoteService = identityRemoteService;
    }

    public async Task<bool> AuthorizeAsync(long id, string policy)
    {
        var cacheKey = GetCacheKey(id);

        // 直接检查权限是否存在
        if (await _redis.SetContainsAsync(cacheKey, policy))
            return true;

        var retryCount = 0;

        if (!_redis.KeyExists(cacheKey))
        {
            while (retryCount < MaxRetryCount)
            {
                // 尝试加载缓存
                var (isLoaded, hasPermission) = await TryLoadCacheWithLockAsync(id, cacheKey, policy);
                if (isLoaded)
                    return hasPermission;

                retryCount++;
                await Task.Delay(100 * retryCount);
            }
        }

        return false; // 降级策略：拒绝访问或走数据库查询
    }
    public async Task ClearCacheAsync(params long[] userIds)
    {
        if (userIds == null || userIds.Length == 0) return;

        var cacheKeys = userIds.Select(GetCacheKey).ToArray();
        var lockKeys = cacheKeys.Select(GetLockKey).ToArray();

        // 使用 Redis 管道批量操作
        var batch = _redis.CreateBatch();

        // 批量删除主缓存键
        var delTask = batch.KeyDeleteAsync(cacheKeys.Select(k => (RedisKey)k).ToArray());

        // 批量删除锁键（预防死锁）
        var delLockTask = batch.KeyDeleteAsync(lockKeys.Select(k => (RedisKey)k).ToArray());

        // 执行管道
        batch.Execute();

        await Task.WhenAll(delTask, delLockTask);
    }

    #region Private Methods
    private async Task<(bool IsLoaded, bool HasPermission)> TryLoadCacheWithLockAsync(
        long userId,
        string cacheKey,
        string policy)
    {
        var lockKey = GetLockKey(cacheKey);
        var lockToken = Guid.NewGuid().ToString();

        try
        {
            if (!await AcquireLockAsync(lockKey, lockToken))
                return (false, false);

            // 二次检查
            if (await _redis.SetContainsAsync(cacheKey, policy))
                return (true, true);

            var permissions = await GetCurrentUserPermissions();
            await HandlePermissionUpdateAsync(cacheKey, permissions);

            return (true, permissions.Contains(policy));
        }
        finally
        {
            await ReleaseLockAsync(lockKey, lockToken);
        }
    }

    private async Task HandlePermissionUpdateAsync(string cacheKey, IEnumerable<string> permissions)
    {
        if (permissions.Any())
        {
            var values = permissions.Select(p => (RedisValue)p).ToArray();
            await _redis.SetAddAsync(cacheKey, values);
            await _redis.KeyExpireAsync(cacheKey, CacheExpiration);
        }
        else
        {
            // 空值处理策略
            await _redis.StringSetAsync(cacheKey, "", expiry: TimeSpan.FromSeconds(30));
        }
    }

    private async Task<bool> AcquireLockAsync(string lockKey, string token)
        => await _redis.StringSetAsync(
            lockKey,
            token,
            expiry: LockTimeout,
            when: When.NotExists);

    private async Task ReleaseLockAsync(string lockKey, string token)
    {
        var script = "if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";
        await _redis.ScriptEvaluateAsync(script, new[] { (RedisKey)lockKey }, new[] { (RedisValue)token });
    }

    private static string GetCacheKey(long userId) => $"{IDENTITY_PERMISSIONS_KEY}{userId}";
    private static string GetLockKey(string cacheKey) => $"{cacheKey}{LOCK_SUFFIX}";

    private async Task<HashSet<string>> GetCurrentUserPermissions()
    {
        // 这里模拟从数据库加载权限
        var permissions = await _identityRemoteService.GetUserPermissionsAsync();
        return [.. permissions];
    }
    #endregion
}