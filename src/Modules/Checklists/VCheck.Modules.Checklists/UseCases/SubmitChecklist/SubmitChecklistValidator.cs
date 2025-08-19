using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.SubmitChecklist
{
    public class SubmitChecklistValidator : AbstractValidator<SubmitChecklistCommand>
    {
        public SubmitChecklistValidator()
        {
            RuleFor(x => x.RowVersion)
                .NotEmpty().WithMessage("O RowVersion não pode ser vazio.")
                .Must(rowVersion => rowVersion.Length > 0).WithMessage("O RowVersion deve conter dados válidos.");
        }
    }
}
