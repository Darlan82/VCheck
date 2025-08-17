using Microsoft.EntityFrameworkCore;

namespace VCheck.Modules.Checklists.Data
{
    public class ChecklistsDbContext : DbContext
    {
        public ChecklistsDbContext(DbContextOptions<ChecklistsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Checklist> Checklists { get; set; } = null!;
        public DbSet<ChecklistItem> ChecklistItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("checklists");

            modelBuilder.Entity<Checklist>(e => {
                e.HasKey(c => c.Id);

                e.Property(c => c.VehicleId).IsRequired();
                e.HasIndex(c => c.VehicleId);

                e.Property(c => c.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<ChecklistItem>(e => {
                e.HasKey(c => c.Id);

                e.Property(c => c.Description).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
