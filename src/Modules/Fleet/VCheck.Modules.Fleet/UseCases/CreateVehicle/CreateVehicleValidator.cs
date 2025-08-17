using FluentValidation;

namespace VCheck.Modules.Fleet.UseCases.CreateVehicle
{
    public class CreateVehicleValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleValidator()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("A placa não pode ser vazia.")
                .Length(7).WithMessage("A placa deve ter exatamente 7 caracteres.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("O modelo não pode ser vazio.");

            RuleFor(x => x.Year)
                .GreaterThan(1950).WithMessage("O ano deve ser maior que 1950.");
        }
    }
}
