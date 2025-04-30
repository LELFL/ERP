using ELF.Data;
using ELF.Infrastructure.Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories;
internal class UserRepository : EFCoreRepository<User, long>, IUserRepository
{
    public UserRepository(ApplicationDbContext db) : base(db) { }

    public async Task RoleAsync(long roleId, string[] permissionIds)
    {
        var entity = _db.Users.Include(x => x.Roles).FirstOrDefault(x => x.Id == roleId) ?? throw new NotFoundException(roleId.ToString(), "User");
        //倒叙删除Roles
        for (int i = entity.Roles.Count - 1; i >= 0; i--)
        {
            entity.Roles.Remove(entity.Roles[i]);
        }
        //添加新的角色
        var permissions = await _db.Roles.Where(x => permissionIds.Contains(x.Id.ToString())).ToListAsync();
        entity.Roles.AddRange(permissions);
        await _db.SaveChangesAsync();
    }
}
