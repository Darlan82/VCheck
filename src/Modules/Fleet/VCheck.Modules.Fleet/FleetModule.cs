using VCheck.Modules.Fleet.Data;
using VCheck.Modules.Fleet.Entidades;
using VCheck.SharedKernel;

namespace VCheck.Modules.Fleet
{
    public class FleetModule : IFleetModule
    {
        private readonly FleetDbContext _dbContext;

        public FleetModule(FleetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsVehicleAvailableForChecklist(Guid vehicleId)
        {
            var vehicle = await _dbContext.Set<Vehicle>().FindAsync(vehicleId);
            if (vehicle == null)
                return false;
            return vehicle.Status == VehicleStatus.Disponivel;
        }

        public async Task<Guid> CreateVehicle(string licensePlate, string model, int year)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                LicensePlate = licensePlate,
                Model = model,
                Year = year,
                Status = VehicleStatus.Disponivel
            };
            _dbContext.Set<Vehicle>().Add(vehicle);
            await _dbContext.SaveChangesAsync();
            return vehicle.Id;
        }
    }
}
