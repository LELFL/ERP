using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Services;

internal class IdentityService : IIdentityService
{
    private readonly IRepository<User, long> _repository;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public IdentityService(IRepository<User, long> repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<bool> AuthorizeAsync(long id, string policy)
    {
        var ids = await GetUserPermissionsWithCache(id);
        if (ids.Contains(policy))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<HashSet<string>> GetUserPermissionsWithCache(long userId)
    {
        var cacheKey = $"user_permissions_{userId}";

        if (!_cache.TryGetValue(cacheKey, out HashSet<string>? permissions))
        {
            permissions = await GetUserPermissionIds(userId);
            _cache.Set(cacheKey, permissions, CacheExpiration);
        }

        return permissions ?? new HashSet<string>();
    }

    private async Task<HashSet<string>> GetUserPermissionIds(long id)
    {
        var ids = await _repository.AsNoTracking().Where(x => x.Id == id).SelectMany(x => x.Roles).SelectMany(x => x.Permissions).Select(x => x.Name).Distinct().ToArrayAsync();
        return [.. ids];
    }
}
