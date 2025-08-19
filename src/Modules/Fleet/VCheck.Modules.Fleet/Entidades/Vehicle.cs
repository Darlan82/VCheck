namespace VCheck.Modules.Fleet.Entidades
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public VehicleStatus Status { get; set; } = VehicleStatus.Disponivel;
    }
}
