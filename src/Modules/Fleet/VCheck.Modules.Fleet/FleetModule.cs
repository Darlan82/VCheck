using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VCheck.Modules.Fleet.Data;
using VCheck.SharedKernel;
//using VCheck.Api.Data;

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
            return vehicle.Status == "Dispon�vel";
        }

        public async Task<Guid> CreateVehicle(string licensePlate, string model, int year)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                LicensePlate = licensePlate,
                Model = model,
                Year = year,
                Status = "Dispon�vel"
            };
            _dbContext.Set<Vehicle>().Add(vehicle);
            await _dbContext.SaveChangesAsync();
            return vehicle.Id;
        }
    }
}
