using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VCheck.Modules.Checklists
{
    public class Checklist
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public string Status { get; set; } = "Pendente";
        public Guid? ExecutorId { get; set; }
        public Guid? SupervisorId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
