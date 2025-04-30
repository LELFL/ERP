using Entities;
using Interfaces;

namespace Repositories;
public interface IRoleRepository : IRepository<Role, long>
{
    Task PermissionAsync(long roleId, string[] permissionIds);
}
