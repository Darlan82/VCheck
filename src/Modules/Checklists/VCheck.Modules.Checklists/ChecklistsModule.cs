using Microsoft.EntityFrameworkCore;
using VCheck.Modules.Checklists.Data;
using VCheck.SharedKernel;

namespace VCheck.Modules.Checklists
{
    public class ChecklistsModule : IChecklistsModule
    {
        private readonly ChecklistsDbContext _dbContext;
        private readonly IFleetModule _fleetModule;

        public ChecklistsModule(ChecklistsDbContext dbContext, IFleetModule fleetModule)
        {
            _dbContext = dbContext;
            _fleetModule = fleetModule;
        }

        public async Task<Guid> StartNewChecklist(Guid vehicleId, Guid executorId)
        {
            if (!await _fleetModule.IsVehicleAvailableForChecklist(vehicleId))
                throw new InvalidOperationException("Ve�culo n�o dispon�vel para checklist.");

            var checklist = new Checklist
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                Status = ChecklistStatus.EmAndamento,
                ExecutorId = executorId,
                StartedAt = DateTime.UtcNow,
                Items = new[]
                {
                    new ChecklistItem { Id = Guid.NewGuid(), Description = "Far�is", Status = ChecklistItemStatus.NaoVerificado },
                    new ChecklistItem { Id = Guid.NewGuid(), Description = "N�vel do �leo", Status = ChecklistItemStatus.NaoVerificado },
                    new ChecklistItem { Id = Guid.NewGuid(), Description = "Pneus", Status = ChecklistItemStatus.NaoVerificado }
                }.ToList()
            };
            _dbContext.Set<Checklist>().Add(checklist);
            await _dbContext.SaveChangesAsync();
            return checklist.Id;
        }

        public async Task UpdateChecklistItem(Guid checklistId, Guid itemId, SharedKernel.ChecklistItemStatus status, string? observations, byte[] rowVersion, Guid executorId)
        {
            var checklist = await _dbContext.Set<Checklist>().Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklist == null) throw new InvalidOperationException("Checklist n�o encontrado.");
            if (checklist.ExecutorId != executorId) throw new InvalidOperationException("Executor inv�lido.");

            _dbContext.Entry(checklist).Property(c => c.RowVersion).OriginalValue = rowVersion;
            var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new InvalidOperationException("Item n�o encontrado.");

            item.Status = (ChecklistItemStatus) status;
            item.Observations = observations;

            await _dbContext.SaveChangesAsync();            
        }

        public async Task SubmitForApproval(Guid checklistId, byte[] rowVersion, Guid executorId)
        {
            var checklist = await _dbContext.Set<Checklist>().FirstOrDefaultAsync(c => c.Id == checklistId);
            if (checklist == null) throw new InvalidOperationException("Checklist n�o encontrado.");
            if (checklist.ExecutorId != executorId) throw new InvalidOperationException("Executor inv�lido.");
            _dbContext.Entry(checklist).Property(c => c.RowVersion).OriginalValue = rowVersion;
            checklist.Status = ChecklistStatus.AguardandoAprovacao;
            checklist.FinishedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();            
        }

        public async Task ApproveChecklist(Guid checklistId, Guid supervisorId)
        {
            var checklist = await _dbContext.Set<Checklist>().FirstOrDefaultAsync(c => c.Id == checklistId);
            if (checklist == null) throw new InvalidOperationException("Checklist n�o encontrado.");
            checklist.SupervisorId = supervisorId;
            checklist.Status = ChecklistStatus.Aprovado;
            
            await _dbContext.SaveChangesAsync();            
        }
    }
}
