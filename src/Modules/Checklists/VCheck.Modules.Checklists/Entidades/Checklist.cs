using System.ComponentModel.DataAnnotations;

namespace VCheck.Modules.Checklists.Entidades
{
    public class Checklist
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public ChecklistStatus Status { get; set; } = ChecklistStatus.Pendente;
        public Guid? ExecutorId { get; set; }
        public Guid? SupervisorId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
