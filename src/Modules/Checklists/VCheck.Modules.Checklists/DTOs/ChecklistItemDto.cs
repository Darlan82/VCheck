namespace VCheck.Modules.Checklists.DTOs
{
    public record ChecklistItemDto(
        Guid Id,
        string Description,
        string Status,
        string? Observations
    );
}
