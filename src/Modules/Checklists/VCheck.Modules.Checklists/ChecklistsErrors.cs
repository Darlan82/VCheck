using VCheck.SharedKernel;

namespace VCheck.Modules.Checklists
{
    public static class ChecklistsErrors
    {
        public static readonly Error VehicleNotAvailableForChecklist = new("checklist.vehicle_not_available", "Veículo não disponível para checklist.");
        public static readonly Error ChecklistNotFound = new("checklist.not_found", "Checklist não encontrado.");
        public static readonly Error InvalidExecutor = new("checklist.invalid_executor", "Executor inválido.");
        public static readonly Error RowVersionConflict = new("checklist.rowversion_conflict", "Versão desatualizada. Consulte o checklist novamente e reenviar a alteração.");
        public static readonly Error ChecklistAlreadySubmitted = new("checklist.already_submitted", "Checklist já submetido para aprovação.");
        public static readonly Error ChecklistNotInProgress = new("checklist.not_in_progress", "Checklist não está em andamento.");
    }
}
