using System;
using System.Threading.Tasks;

namespace VCheck.SharedKernel
{
    public interface IChecklistsModule
    {
        Task<Guid> StartNewChecklist(Guid vehicleId, Guid executorId);
        Task UpdateChecklistItem(Guid checklistId, Guid itemId, string status, string? observations, byte[] rowVersion, Guid executorId);
        Task SubmitForApproval(Guid checklistId, byte[] rowVersion, Guid executorId);
        Task ApproveChecklist(Guid checklistId, Guid supervisorId);
    }
}
