using Microsoft.EntityFrameworkCore;
using USFParkingTracker.Models;

namespace USFParkingTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<ParkingEvent> ParkingEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Coordinates as owned entity
            modelBuilder.Entity<ParkingLot>()
                .OwnsOne(p => p.Coordinates);
                
            // Set up relationships
            modelBuilder.Entity<ParkingEvent>()
                .HasOne(e => e.ParkingLot)
                .WithMany()
                .HasForeignKey(e => e.ParkingLotId);
        }
    }
}