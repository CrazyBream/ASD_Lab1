using ASD_Lab1.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASD_Lab1.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<InstalledSoftwareEntity> InstalledSoftware { get; set; }
        public DbSet<PeripheralEntity> Peripherals { get; set; }
        public DbSet<HardwareComponentEntity> HardwareComponents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeviceEntity>()
                .HasMany(d => d.InstalledSoftware)
                .WithOne(s => s.Device)
                .HasForeignKey(s => s.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceEntity>()
                .HasMany(d => d.ConnectedPeripherals)
                .WithOne(p => p.Device)
                .HasForeignKey(p => p.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceEntity>()
                .HasMany(d => d.HardwareComponents)
                .WithOne(h => h.Device)
                .HasForeignKey(h => h.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}