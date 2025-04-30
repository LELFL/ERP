using ELF.Data;
using ELF.Infrastructure.Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories;
internal class RoleRepository : EFCoreRepository<Role, long>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext db) : base(db) { }

    public async Task PermissionAsync(long roleId, string[] permissionIds)
    {
        var entity = _db.Roles.Include(x => x.Permissions).FirstOrDefault(x => x.Id == roleId) ?? throw new NotFoundException(roleId.ToString(), "Role");
        //倒叙删除Premissions
        for (int i = entity.Permissions.Count - 1; i >= 0; i--)
        {
            entity.Permissions.Remove(entity.Permissions[i]);
        }
        //添加新的权限
        var permissions = await _db.Permissions.Where(x => permissionIds.Contains(x.Id.ToString())).ToListAsync();
        entity.Permissions.AddRange(permissions);
        await _db.SaveChangesAsync();
    }
}
