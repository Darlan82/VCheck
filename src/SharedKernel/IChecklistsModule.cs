namespace VCheck.SharedKernel
{   
    public interface IChecklistsModule
    {
        Task<(Guid?, Error)> StartNewChecklist(Guid vehicleId, Guid executorId);
        Task<Error> UpdateChecklistItem(Guid checklistId, Guid itemId, ChecklistItemStatus status, string? observations, byte[] rowVersion, Guid executorId);
        Task<Error> SubmitForApproval(Guid checklistId, byte[] rowVersion, Guid executorId);
        Task<Error> ApproveChecklist(Guid checklistId, Guid supervisorId);
    }
}
