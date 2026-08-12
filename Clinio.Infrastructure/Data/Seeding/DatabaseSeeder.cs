using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Entities.Clinics;
using Clinio.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Infrastructure.Data.Seeding;

public class DatabaseSeeder(
    IApplicationDbContext _context,
    UserManager<ApplicationUser> _userManager,
    ITimeSlotGenerator _slotGenerator
) : IDatabaseSeeder
{
    private const int DefaultDaysAhead = 30;

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (await _context.Doctors.AnyAsync()) return;

        // ── Clinics ──────────────────────────────────────────────
        var clinics = new List<Clinic>
        {
            new() { Name = "Al Shifa Clinic",      Address = "Cairo - Nasr City",      PhoneNumber = "01000000001" },
            new() { Name = "Life Care Clinic",      Address = "Giza - Dokki",           PhoneNumber = "01000000002" },
            new() { Name = "Nile Medical Center",   Address = "Cairo - Maadi",          PhoneNumber = "01000000003" },
            new() { Name = "Salam Hospital",        Address = "Alexandria - Smouha",    PhoneNumber = "01000000004" },
            new() { Name = "Al Rahma Clinic",       Address = "Cairo - Heliopolis",     PhoneNumber = "01000000005" },
        };

        await _context.Clinics.AddRangeAsync(clinics, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // ── Doctor Users ─────────────────────────────────────────
        var doctorData = new[]
        {
            new { First = "Ahmed",   Last = "Hassan",    Email = "ahmed.doctor@test.com",   Username = "ahmed.doctor",   Specialty = "Cardiology",      License = "DOC-1001", Fee = 500m,  ClinicIndex = 0 },
            new { First = "Sara",    Last = "Mohamed",   Email = "sara.doctor@test.com",    Username = "sara.doctor",    Specialty = "Dermatology",     License = "DOC-1002", Fee = 300m,  ClinicIndex = 1 },
            new { First = "Omar",    Last = "Ali",       Email = "omar.doctor@test.com",    Username = "omar.doctor",    Specialty = "Orthopedics",     License = "DOC-1003", Fee = 450m,  ClinicIndex = 2 },
            new { First = "Nadia",   Last = "Kamal",     Email = "nadia.doctor@test.com",   Username = "nadia.doctor",   Specialty = "Pediatrics",      License = "DOC-1004", Fee = 350m,  ClinicIndex = 3 },
            new { First = "Khaled",  Last = "Ibrahim",   Email = "khaled.doctor@test.com",  Username = "khaled.doctor",  Specialty = "Neurology",       License = "DOC-1005", Fee = 600m,  ClinicIndex = 4 },
            new { First = "Dina",    Last = "Youssef",   Email = "dina.doctor@test.com",    Username = "dina.doctor",    Specialty = "Gynecology",      License = "DOC-1006", Fee = 400m,  ClinicIndex = 0 },
            new { First = "Tarek",   Last = "Mahmoud",   Email = "tarek.doctor@test.com",   Username = "tarek.doctor",   Specialty = "Ophthalmology",   License = "DOC-1007", Fee = 350m,  ClinicIndex = 1 },
            new { First = "Mona",    Last = "Fathy",     Email = "mona.doctor@test.com",    Username = "mona.doctor",    Specialty = "Psychiatry",      License = "DOC-1008", Fee = 550m,  ClinicIndex = 2 },
            new { First = "Hassan",  Last = "Nasser",    Email = "hassan.doctor@test.com",  Username = "hassan.doctor",  Specialty = "Urology",         License = "DOC-1009", Fee = 420m,  ClinicIndex = 3 },
            new { First = "Rana",    Last = "Samir",     Email = "rana.doctor@test.com",    Username = "rana.doctor",    Specialty = "Endocrinology",   License = "DOC-1010", Fee = 480m,  ClinicIndex = 4 },
        };

        var users = new List<ApplicationUser>();

        foreach (var d in doctorData)
        {
            var user = new ApplicationUser
            {
                FirstName = d.First,
                LastName = d.Last,
                UserName = d.Username,
                Email = d.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "Doctor@123");
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Doctor");
            users.Add(user);
        }

        // ── Doctor Profiles ───────────────────────────────────────
        var doctors = doctorData.Select((d, i) => new Doctor
        {
            Specialty = d.Specialty,
            LicenseNumber = d.License,
            ConsultationFee = d.Fee,
            UserId = users[i].Id,
            ClinicId = clinics[d.ClinicIndex].Id
        }).ToList();

        await _context.Doctors.AddRangeAsync(doctors, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // ── Schedules + Slots ─────────────────────────────────────
        var schedules = new List<DoctorSchedule>();

        foreach (var doctor in doctors)
        {
            // each doctor works Sat, Mon, Wed — 9am to 5pm, 30 min slots
            var workingDays = new[] { DayOfWeek.Saturday, DayOfWeek.Monday, DayOfWeek.Wednesday };

            foreach (var day in workingDays)
            {
                schedules.Add(new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    ClinicId = doctor.ClinicId,
                    DayOfWeek = day,
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(17, 0),
                    SlotDurationMinutes = 30,
                    IsActive = true
                });
            }
        }

        await _context.DoctorSchedules.AddRangeAsync(schedules, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // ── Generate Time Slots ───────────────────────────────────
        var allSlots = new List<TimeSlot>();

        foreach (var schedule in schedules)
        {
            var slots = _slotGenerator.GenerateSlots(
                schedule,
                DefaultDaysAhead,
                new HashSet<(DateOnly, TimeOnly)>()
            );
            allSlots.AddRange(slots);
        }

        await _context.TimeSlots.AddRangeAsync(allSlots, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}