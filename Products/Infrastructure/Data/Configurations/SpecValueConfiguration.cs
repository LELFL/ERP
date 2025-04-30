using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELF.Infrastructure.Data.Configurations;

public class SpecValueConfiguration : IEntityTypeConfiguration<SpecValue>
{
    public void Configure(EntityTypeBuilder<SpecValue> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.SpecId)
        .IsRequired()
        ;
        builder.Property(t => t.Value)
        .HasMaxLength(100)
        .IsRequired()
        ;
        builder.Property(t => t.Sort)
        .IsRequired()
        ;
    }
}