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
               .WithMessage($"A observa��o deve ter no m�ximo {ChecklistsConstants.ChecklistItem.ObservationsMaxLength} caracteres.")
               ;

            RuleFor(x => x.RowVersion)
                .NotNull().WithMessage("RowVersion n�o pode ser nulo.");
        }
    }
}
