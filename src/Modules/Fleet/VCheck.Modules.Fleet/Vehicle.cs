using System;

namespace VCheck.Modules.Fleet
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Status { get; set; } = "Dispon�vel";
    }
}
