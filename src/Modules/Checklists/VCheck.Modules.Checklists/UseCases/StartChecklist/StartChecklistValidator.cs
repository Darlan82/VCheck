using FluentValidation;

namespace VCheck.Modules.Checklists.UseCases.StartChecklist
{
    public class StartChecklistValidator : AbstractValidator<StartChecklistCommand>
    {
        public StartChecklistValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("O VehicleId n�o pode ser vazio.");
        }
    }
}
