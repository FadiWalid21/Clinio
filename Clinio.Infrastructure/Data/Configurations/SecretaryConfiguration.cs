using Clinio.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinio.Infrastructure.Data.Configurations;

public class SecretaryConfiguration : IEntityTypeConfiguration<Secretary>
{
    public void Configure(EntityTypeBuilder<Secretary> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.ApplicationUser)
            .WithOne(u => u.SecretaryProfile)
            .HasForeignKey<Secretary>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Clinic)
            .WithMany(c => c.Secretaries)
            .HasForeignKey(s => s.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}