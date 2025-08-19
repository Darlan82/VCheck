using Microsoft.EntityFrameworkCore;
using VCheck.Modules.Checklists.Data;
using VCheck.Modules.Checklists.DTOs;
using VCheck.Modules.Checklists.Entidades;

namespace VCheck.Modules.Checklists
{
    internal class ChecklistsQueries(ChecklistsDbContext _dbContext) : IChecklistsQueries
    {
        public async Task<ChecklistDto?> GetChecklist(Guid checklistId)
        {
            var checklist = await _dbContext.Set<Checklist>()
                .Include(c => c.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklist == null) return null;

            return new ChecklistDto(
                checklist.Id,
                checklist.VehicleId,
                checklist.Status.ToString(),
                checklist.ExecutorId,
                checklist.SupervisorId,
                checklist.StartedAt,
                checklist.FinishedAt,
                checklist.RowVersion,
                checklist.Items.Select(i => new ChecklistItemDto(i.Id, i.Description, i.Status.ToString(), i.Observations)).ToList()
            );
        }
    }
}
