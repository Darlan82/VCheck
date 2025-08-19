using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public class UpdateChecklistItemValidator : AbstractValidator<UpdateChecklistItemCommand>
    {
        public UpdateChecklistItemValidator()
        {
            RuleFor(x => x.Observations)
               .MaximumLength(ChecklistsConstants.ChecklistItem.ObservationsMaxLength)
               .WithMessage($"A observação deve ter no máximo {ChecklistsConstants.ChecklistItem.ObservationsMaxLength} caracteres.")
               ;

            RuleFor(x => x.ChecklistsRowVersion)
                .NotEmpty().WithMessage("O RowVersion não pode ser vazio.")
                .Must(rowVersion => rowVersion.Length > 0).WithMessage("O RowVersion deve conter dados válidos.");
        }
    }
}
