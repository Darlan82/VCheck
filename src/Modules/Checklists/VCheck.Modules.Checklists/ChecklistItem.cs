namespace VCheck.Modules.Checklists
{
    public class ChecklistItem
    {
        public Guid Id { get; set; }
        public Guid ChecklistId { get; set; }
        public string Description { get; set; } = string.Empty;
        public ChecklistItemStatus Status { get; set; } = ChecklistItemStatus.NaoVerificado;
        public string? Observations { get; set; }
        public Checklist Checklist { get; set; } = null!;
    }
}
