namespace Interfaces;

public interface IIdentityRemoteService
{
    Task<string[]> GetUserPermissionsAsync();
}
