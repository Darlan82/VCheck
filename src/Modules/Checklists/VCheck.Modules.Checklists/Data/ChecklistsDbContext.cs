using Microsoft.EntityFrameworkCore;
using VCheck.Modules.Checklists.Entidades;

namespace VCheck.Modules.Checklists.Data
{
    public class ChecklistsDbContext : DbContext
    {
        public const string schema = "checklists";

        public ChecklistsDbContext(DbContextOptions<ChecklistsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Checklist> Checklists { get; set; } = null!;
        public DbSet<ChecklistItem> ChecklistItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schema);

            modelBuilder.Entity<Checklist>(e => {
                e.HasKey(c => c.Id);

                e.Property(c => c.VehicleId).IsRequired();
                e.HasIndex(c => c.VehicleId);

                e.Property(c => c.Status)
                    .HasConversion<short>()
                    .HasColumnType("smallint")
                    .IsRequired();

                e.Property(c => c.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<ChecklistItem>(e => {
                e.HasKey(c => c.Id);

                e.Property(c => c.Description)
                    .HasMaxLength(ChecklistsConstants.ChecklistItem.DescriptionMaxLength)
                    .IsRequired();

                e.Property(c => c.Observations)
                    .HasMaxLength(ChecklistsConstants.ChecklistItem.ObservationsMaxLength);

                e.Property(c => c.Status)
                    .HasConversion<short>()
                    .HasColumnType("smallint")
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
