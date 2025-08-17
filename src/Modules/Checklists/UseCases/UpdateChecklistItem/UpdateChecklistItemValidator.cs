using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public class UpdateChecklistItemValidator : AbstractValidator<UpdateChecklistItemCommand>
    {
        private static readonly string[] AllowedStatuses = { "Conforme", "NaoConforme", "NaoVerificado" };

        public UpdateChecklistItemValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status n�o pode ser vazio.")
                .Must(s => AllowedStatuses.Contains(s)).WithMessage("Status inv�lido.");

            RuleFor(x => x.RowVersion)
                .NotNull().WithMessage("RowVersion n�o pode ser nulo.");
        }
    }
}
