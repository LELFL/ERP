using Entities;
using Interfaces;

namespace Repositories;
public interface IUserRepository : IRepository<User, long>
{
    Task RoleAsync(long roleId, string[] permissionIds);
}
