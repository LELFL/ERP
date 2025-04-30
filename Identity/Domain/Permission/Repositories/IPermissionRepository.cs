using Entities;
using Interfaces;

namespace Repositories;
public interface IPermissionRepository : IRepository<Permission, long>
{
    Task<List<Permission>> GetTreeListAsync();
}
