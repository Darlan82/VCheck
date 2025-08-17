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
            base.OnModelCreating(modelBuilder);
        }
    }
}
