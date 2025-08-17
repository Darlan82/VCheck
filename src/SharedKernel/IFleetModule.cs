using System;
using System.Threading.Tasks;

namespace VCheck.SharedKernel
{
    public interface IFleetModule
    {
        Task<bool> IsVehicleAvailableForChecklist(Guid vehicleId);
        Task<Guid> CreateVehicle(string licensePlate, string model, int year);
    }
}
