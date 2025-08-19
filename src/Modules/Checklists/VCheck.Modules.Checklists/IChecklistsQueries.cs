using VCheck.Modules.Checklists.DTOs;

namespace VCheck.Modules.Checklists
{
    public interface IChecklistsQueries
    {
        Task<ChecklistDto?> GetChecklist(Guid checklistId);
    }
}
