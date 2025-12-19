using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Models.Entities;

namespace RandevuYonetimSistemi.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Gym> Gyms => Set<Gym>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<TrainerAvailability> TrainerAvailabilities => Set<TrainerAvailability>();
    public DbSet<TrainerServiceMap> TrainerServiceMaps => Set<TrainerServiceMap>();
    public DbSet<TrainerSpecialtyMap> TrainerSpecialtyMaps => Set<TrainerSpecialtyMap>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // N-N composite keys
        builder.Entity<TrainerServiceMap>()
            .HasKey(x => new { x.TrainerId, x.ServiceId });

        builder.Entity<TrainerSpecialtyMap>()
            .HasKey(x => new { x.TrainerId, x.SpecialtyId });

        // Gym relationships
        builder.Entity<Service>()
            .HasOne(s => s.Gym)
            .WithMany(g => g.Services)
            .HasForeignKey(s => s.GymId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Trainer>()
            .HasOne(t => t.Gym)
            .WithMany(g => g.Trainers)
            .HasForeignKey(t => t.GymId)
            .OnDelete(DeleteBehavior.Cascade);

        // TrainerAvailability
        builder.Entity<TrainerAvailability>()
            .HasOne(a => a.Trainer)
            .WithMany(t => t.Availabilities)
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Trainer <-> Service map
        builder.Entity<TrainerServiceMap>()
            .HasOne(x => x.Trainer)
            .WithMany(t => t.TrainerServiceMaps)
            .HasForeignKey(x => x.TrainerId);

        builder.Entity<TrainerServiceMap>()
            .HasOne(x => x.Service)
            .WithMany(s => s.TrainerServiceMaps)
            .HasForeignKey(x => x.ServiceId);

        // Trainer <-> Specialty map
        builder.Entity<TrainerSpecialtyMap>()
            .HasOne(x => x.Trainer)
            .WithMany(t => t.TrainerSpecialtyMaps)
            .HasForeignKey(x => x.TrainerId);

        builder.Entity<TrainerSpecialtyMap>()
            .HasOne(x => x.Specialty)
            .WithMany(s => s.TrainerSpecialtyMaps)
            .HasForeignKey(x => x.SpecialtyId);

        builder.Entity<Appointment>()
            .HasOne(a => a.Member)
            .WithMany() // istersen ApplicationUser'a ICollection<Appointment> eklersin
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.Trainer)
            .WithMany(t => t.Appointments)
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Appointment>()
            .HasOne(a => a.Gym)
            .WithMany()
            .HasForeignKey(a => a.GymId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // PostgreSQL decimal mapping (sağlam olsun)
        builder.Entity<Service>()
            .Property(x => x.Price)
            .HasColumnType("numeric(10,2)");

        builder.Entity<Appointment>()
            .Property(x => x.PriceSnapshot)
            .HasColumnType("numeric(10,2)");
        builder.Entity<Appointment>()
    .Property(a => a.Status)
    .HasConversion<string>();
    }
}
