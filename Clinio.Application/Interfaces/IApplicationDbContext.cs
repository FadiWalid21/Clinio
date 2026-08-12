using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Entities.Clinics;
using Clinio.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Clinio.Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Secretary> Secretaries { get; set; }
    public DbSet<ClinicImage> ClinicImages { get; set; }
    public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    public DbSet<TimeSlot> TimeSlots { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
}