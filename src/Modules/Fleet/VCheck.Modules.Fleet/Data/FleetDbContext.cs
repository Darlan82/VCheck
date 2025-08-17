using Microsoft.EntityFrameworkCore;
using VCheck.Modules.Fleet;

namespace VCheck.Modules.Fleet.Data
{
    public class FleetDbContext : DbContext
    {
        public FleetDbContext(DbContextOptions<FleetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("fleet");

            modelBuilder.Entity<Vehicle>(e => {
                e.HasKey(c => c.Id);

                e.Property(c => c.LicensePlate)
                    .HasMaxLength(FleetConstants.Vehicle.LicensePlateLength)
                    .IsRequired();

                e.Property(c => c.Model)
                    .HasMaxLength(FleetConstants.Vehicle.ModelMaxLength)
                    .IsRequired();

                e.Property(c => c.Status)
                    .HasConversion<int>()
                    .HasColumnType("smallint")
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
