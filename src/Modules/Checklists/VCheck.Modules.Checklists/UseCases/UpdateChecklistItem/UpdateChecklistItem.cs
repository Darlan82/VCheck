namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public record UpdateChecklistItemCommand(string Status, string? Observations, byte[] RowVersion);
}
