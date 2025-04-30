using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace ELF.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        this.ChangeTracker.Tracking += OnTracking;
    }
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void OnTracking(object? sender, EntityTrackingEventArgs e)
    {
        if (e.Entry.Entity is IEntity<long> longEntity && longEntity.Id == 0)
        {
            longEntity.Id = Yitter.IdGenerator.YitIdHelper.NextId();
        }
    }
}
