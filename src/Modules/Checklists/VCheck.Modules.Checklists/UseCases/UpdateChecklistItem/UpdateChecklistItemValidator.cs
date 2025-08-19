using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.UpdateChecklistItem
{
    public class UpdateChecklistItemValidator : AbstractValidator<UpdateChecklistItemCommand>
    {
        public UpdateChecklistItemValidator()
        {
            RuleFor(x => x.Observations)
               .MaximumLength(ChecklistsConstants.ChecklistItem.ObservationsMaxLength)
               .WithMessage($"A observa��o deve ter no m�ximo {ChecklistsConstants.ChecklistItem.ObservationsMaxLength} caracteres.")
               ;

            RuleFor(x => x.ChecklistsRowVersion)
                .NotEmpty().WithMessage("O RowVersion n�o pode ser vazio.")
                .Must(rowVersion => rowVersion.Length > 0).WithMessage("O RowVersion deve conter dados v�lidos.");
        }
    }
}
