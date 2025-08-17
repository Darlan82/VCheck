using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public class UpdateChecklistItemValidator : AbstractValidator<UpdateChecklistItemCommand>
    {
        private static readonly string[] AllowedStatuses = { "Conforme", "NaoConforme", "NaoVerificado" };

        public UpdateChecklistItemValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status não pode ser vazio.")
                .Must(s => AllowedStatuses.Contains(s)).WithMessage("Status inválido.");

            RuleFor(x => x.RowVersion)
                .NotNull().WithMessage("RowVersion não pode ser nulo.");
        }
    }
}
