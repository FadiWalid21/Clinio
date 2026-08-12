using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Entities.Clinics;
using Clinio.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Infrastructure.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Clinic> Clinics { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Secretary> Secretaries { get; set; } = null!;
    public DbSet<ClinicImage> ClinicImages { get; set; } = null!;
    public DbSet<DoctorSchedule> DoctorSchedules { get; set; } = null!;
    public DbSet<TimeSlot> TimeSlots { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var adminRoleId = 1;
        var doctorRoleId = 2;
        var patientRoleId = 3;
        var secretaryRoleId = 4;

        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new ApplicationRole { Id = doctorRoleId, Name = "Doctor", NormalizedName = "DOCTOR" },
            new ApplicationRole { Id = patientRoleId, Name = "Patient", NormalizedName = "PATIENT" },
            new ApplicationRole { Id = secretaryRoleId, Name = "Secretary", NormalizedName = "SECRETARY" }
        );

        var adminUserId = 1;
        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            FirstName = "Fadi",
            LastName = "Walid",
            UserName = "fadi.walid",
            NormalizedUserName = "FADI.WALID",
            Email = "fadiwalid2002@gmail.com",
            NormalizedEmail = "FADIWALID2002@GMAIL.COM",
            EmailConfirmed = true,
            SecurityStamp = "fadi-admin-static-security-stamp-123"
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
        {
            UserId = adminUserId,
            RoleId = adminRoleId
        });
        
        // According to make sure the ar lang will work
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var stringProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string));

            foreach (var property in stringProperties)
            {
                property.SetIsUnicode(true); 
            }
        }
        
        modelBuilder.Entity<ApplicationUser>(b =>
        {
            b.HasIndex(c => c.FirstName);
            b.HasIndex(c => c.LastName);
            
            b.Property(u => u.Image).HasMaxLength(500);
            b.Property(u => u.ImageFileName).HasMaxLength(500);
            
            b.OwnsMany(u => u.RefreshTokens, rt =>
            {
                rt.ToTable("UserRefreshTokens");

                rt.HasKey(t => t.Token);
            
                rt.WithOwner().HasForeignKey("UserId");
            });
        });
        
        // Fluent API Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}