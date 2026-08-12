using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinio.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.Notes).HasMaxLength(500);
        builder.Property(a => a.CancellationReason).HasMaxLength(500);
        builder.Property(a => a.CreatedAt).IsRequired();

        // one timeslot → one appointment max
        builder.HasIndex(a => a.TimeSlotId).IsUnique();

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Clinic)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TimeSlot)
            .WithOne(t => t.Appointment)
            .HasForeignKey<Appointment>(a => a.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.BookedBy)
            .WithMany()
            .HasForeignKey(a => a.BookedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.CancelledBy)
            .WithMany()
            .HasForeignKey(a => a.CancelledById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}