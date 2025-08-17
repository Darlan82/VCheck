using FluentValidation;

namespace VCheck.Modules.Fleet.UseCases.CreateVehicle
{
    public class CreateVehicleValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleValidator()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("A placa n�o pode ser vazia.")
                .Length(FleetConstants.Vehicle.LicensePlateLength)
                .WithMessage($"A placa deve ter exatamente {FleetConstants.Vehicle.LicensePlateLength} caracteres.");

            RuleFor(x => x.Model)                
                .NotEmpty().WithMessage("O modelo n�o pode ser vazio.")
                .MaximumLength(FleetConstants.Vehicle.ModelMaxLength)
                .WithMessage($"O modelo deve ter no m�ximo {FleetConstants.Vehicle.ModelMaxLength} caracteres.")
                ;

            RuleFor(x => x.Year)
                .GreaterThan(1950).WithMessage("O ano deve ser maior que 1950.");
        }
    }
}
