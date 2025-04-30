using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class ProductSKUConfiguration : IEntityTypeConfiguration<ProductSKU>
{
    public void Configure(EntityTypeBuilder<ProductSKU> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.No)
        .HasMaxLength(100)
        .IsRequired()
        ;
        builder.Property(t => t.ProductId)
        .IsRequired()
        ;
        builder.Property(t => t.Weight)
        .HasPrecision(18, 4)
        .IsRequired()
        ;
        builder.Property(t => t.Price)
        .HasPrecision(18, 4)
        .IsRequired()
        ;
        builder.HasMany(t => t.SpecValues)
            .WithMany()
        ;
    }
}