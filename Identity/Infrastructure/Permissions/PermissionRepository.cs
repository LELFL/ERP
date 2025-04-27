using ELF.Data;
using ELF.Infrastructure.Data;
using ELF.Interfaces.Permissions;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories;
internal class PermissionRepository(ApplicationDbContext db) : EFCoreRepository<Permission, long>(db), IPermissionRepository
{
    public Task<List<Permission>> GetTreeListAsync()
    {
        var list = _db.Permissions
            .Include(x => x.Children)
            .Include(x => x.Parent)
            .OrderBy(x => x.Sort)
            .ToListAsync();
        return list;
    }
}
