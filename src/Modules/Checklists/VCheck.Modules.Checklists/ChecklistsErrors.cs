using VCheck.SharedKernel;

namespace VCheck.Modules.Checklists
{
    public static class ChecklistsErrors
    {
        public static readonly Error VehicleNotAvailableForChecklist = new("checklist.vehicle_not_available", "Veículo não disponível para checklist.");
        public static readonly Error ChecklistNotFound = new("checklist.not_found", "Checklist não encontrado.");
        public static readonly Error InvalidExecutor = new("checklist.invalid_executor", "Executor inválido.");
    }
}
