using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name)
        .HasMaxLength(100)
        .IsRequired()
        ;
        builder.Property(t => t.Sort)
        .IsRequired()
        ;
        builder.Property(t => t.Enable)
        .IsRequired()
        ;
    }
}