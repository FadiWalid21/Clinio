using Clinio.Domain.Entities.Clinics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinio.Infrastructure.Data.Configurations;

public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
{
    public void Configure(EntityTypeBuilder<Clinic> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Name);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.HasMany(c => c.Doctors)
            .WithOne(d => d.Clinic)
            .HasForeignKey(d => d.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Secretaries)
            .WithOne(s => s.Clinic)
            .HasForeignKey(s => s.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(c => c.Images)
            .WithOne(i => i.Clinic)
            .HasForeignKey(i => i.ClinicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}