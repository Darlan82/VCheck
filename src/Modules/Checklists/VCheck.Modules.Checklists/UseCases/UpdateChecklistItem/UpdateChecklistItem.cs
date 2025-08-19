namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public record UpdateChecklistItemCommand(SharedKernel.ChecklistItemStatus Status, string? Observations, byte[] ChecklistsRowVersion);
}
