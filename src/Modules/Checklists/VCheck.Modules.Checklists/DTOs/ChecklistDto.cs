namespace VCheck.Modules.Checklists.DTOs
{
    public record ChecklistDto(
        Guid Id,
        Guid VehicleId,
        string Status,
        Guid? ExecutorId,
        Guid? SupervisorId,
        DateTime? StartedAt,
        DateTime? FinishedAt,
        byte[] RowVersion,
        IReadOnlyCollection<ChecklistItemDto> Items
    );
}
