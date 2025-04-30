
namespace Interfaces;
public interface IIdentityService
{
    Task<bool> AuthorizeAsync(long id, string permission);
    Task ClearCacheAsync(params long[] userIds);
}
