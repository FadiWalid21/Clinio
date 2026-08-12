using Clinio.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinio.Infrastructure.Data.Configurations;

public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Date).IsRequired();
        builder.Property(t => t.StartTime).IsRequired();
        builder.Property(t => t.EndTime).IsRequired();
        builder.Property(t => t.IsBooked).HasDefaultValue(false);

        // prevent duplicate slots for same doctor/clinic/date/time
        builder.HasIndex(t => new { t.DoctorId, t.ClinicId, t.Date, t.StartTime })
            .IsUnique();

        builder.HasOne(t => t.Doctor)
            .WithMany(d => d.TimeSlots)
            .HasForeignKey(t => t.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Clinic)
            .WithMany(c => c.TimeSlots)
            .HasForeignKey(t => t.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.DoctorSchedule)
            .WithMany(s => s.TimeSlots)
            .HasForeignKey(t => t.DoctorScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}