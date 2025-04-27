using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(t => t.Description)
            .HasMaxLength(100);

        // 配置角色和权限的多对多关系
        builder.HasMany(role => role.Permissions)
            .WithMany();
    }
}
