using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(t => t.Nickname)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(t => t.Password)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(t => t.Email)
            .HasMaxLength(100);

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(100);

        // 配置用户和角色之间的多对多关系
        builder.HasMany(role => role.Roles)
            .WithMany();
    }
}
