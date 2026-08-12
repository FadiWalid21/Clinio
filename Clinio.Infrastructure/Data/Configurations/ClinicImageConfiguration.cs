using Clinio.Domain.Entities.Clinics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinio.Infrastructure.Data.Configurations;

public class ClinicImageConfiguration : IEntityTypeConfiguration<ClinicImage>
{
    public void Configure(EntityTypeBuilder<ClinicImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.FileName)
            .IsRequired()
            .HasMaxLength(500);
    }
}