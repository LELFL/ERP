
namespace Interfaces;
public interface IIdentityService
{
    Task<bool> AuthorizeAsync(long id, string permission);
}
