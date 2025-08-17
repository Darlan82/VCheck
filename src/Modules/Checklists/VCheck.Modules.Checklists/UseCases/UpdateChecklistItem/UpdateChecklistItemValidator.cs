using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public class UpdateChecklistItemValidator : AbstractValidator<UpdateChecklistItemCommand>
    {
        private static readonly string[] AllowedStatuses = { "Conforme", "NaoConforme", "NaoVerificado" };

        public UpdateChecklistItemValidator()
        {
            RuleFor(x => x.Observations)
               .MaximumLength(ChecklistsConstants.ChecklistItem.ObservationsMaxLength)
               .WithMessage($"A observação deve ter no máximo {ChecklistsConstants.ChecklistItem.ObservationsMaxLength} caracteres.")
               ;

            RuleFor(x => x.RowVersion)
                .NotNull().WithMessage("RowVersion não pode ser nulo.");
        }
    }
}
