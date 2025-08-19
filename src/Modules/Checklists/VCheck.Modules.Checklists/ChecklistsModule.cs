using Microsoft.EntityFrameworkCore;
using VCheck.Modules.Checklists.Data;
using VCheck.Modules.Checklists.Entidades;
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

        public async Task<(Guid?, Error)> StartNewChecklist(Guid vehicleId, Guid executorId)
        {
            if (!await _fleetModule.IsVehicleAvailableForChecklist(vehicleId))
                return (null, ChecklistsErrors.VehicleNotAvailableForChecklist);

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

            return (checklist.Id, Error.None);
        }

        public async Task<Error> UpdateChecklistItem(Guid checklistId, Guid itemId, SharedKernel.ChecklistItemStatus status, string? observations, byte[] rowVersion, Guid executorId)
        {
            var checklist = await _dbContext.Set<Checklist>().Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklist == null) return ChecklistsErrors.ChecklistNotFound;
            if (checklist.ExecutorId != executorId) return ChecklistsErrors.InvalidExecutor;

            _dbContext.Entry(checklist).Property(c => c.RowVersion).OriginalValue = rowVersion;
            var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new InvalidOperationException("Item n�o encontrado.");

            item.Status = (ChecklistItemStatus) status;
            item.Observations = observations;

            await _dbContext.SaveChangesAsync();

            return Error.None;
        }

        public async Task<Error> SubmitForApproval(Guid checklistId, byte[] rowVersion, Guid executorId)
        {
            var checklist = await _dbContext.Set<Checklist>().FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklist == null) return ChecklistsErrors.ChecklistNotFound;
            if (checklist.ExecutorId != executorId) return ChecklistsErrors.InvalidExecutor;

            _dbContext.Entry(checklist).Property(c => c.RowVersion).OriginalValue = rowVersion;
            checklist.Status = ChecklistStatus.AguardandoAprovacao;
            checklist.FinishedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();

            return Error.None;
        }

        public async Task<Error> ApproveChecklist(Guid checklistId, Guid supervisorId)
        {
            var checklist = await _dbContext.Set<Checklist>().FirstOrDefaultAsync(c => c.Id == checklistId);

            if (checklist == null) return ChecklistsErrors.ChecklistNotFound;

            checklist.SupervisorId = supervisorId;
            checklist.Status = ChecklistStatus.Aprovado;
            
            await _dbContext.SaveChangesAsync();

            return Error.None;
        }
    }
}
