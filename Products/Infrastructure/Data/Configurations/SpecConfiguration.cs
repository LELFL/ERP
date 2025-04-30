using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class SpecConfiguration : IEntityTypeConfiguration<Spec>
{
    public void Configure(EntityTypeBuilder<Spec> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name)
        .HasMaxLength(100)
        .IsRequired()
        ;
    }
}