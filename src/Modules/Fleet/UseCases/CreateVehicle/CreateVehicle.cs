namespace VCheck.Modules.Fleet.UseCases.CreateVehicle
{
    public record CreateVehicleCommand(string LicensePlate, string Model, int Year);
}
