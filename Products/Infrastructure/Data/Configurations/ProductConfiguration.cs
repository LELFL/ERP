using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.No)
        .HasMaxLength(100)
        .IsRequired()
        ;
        builder.Property(t => t.Name)
        .HasMaxLength(100)
        .IsRequired()
        ;
        builder.Property(t => t.CategoryId)
        .IsRequired()
        ;
        builder.Property(t => t.BrandId)
        .IsRequired()
        ;
        builder.Property(t => t.Enable)
        .IsRequired()
        ;
        builder.Property(t => t.Style)
        .HasMaxLength(100)
        ;
        builder.Property(t => t.Model)
        .HasMaxLength(100)
        ;
        builder.Property(t => t.Specification)
        .HasMaxLength(100)
        ;
        builder.HasMany(t => t.Specs)
            .WithMany()
        ;
    }
}