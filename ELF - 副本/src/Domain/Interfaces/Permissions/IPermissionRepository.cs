using Entities;
using Interfaces;

namespace ELF.Interfaces.Permissions;
public interface IPermissionRepository : IRepository<Permission, long>
{
    Task<List<Permission>> GetTreeListAsync();
}
