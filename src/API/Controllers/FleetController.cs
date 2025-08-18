using Microsoft.AspNetCore.Mvc;
using VCheck.Modules.Fleet.UseCases.CreateVehicle;
using VCheck.SharedKernel;

namespace VCheck.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FleetController : ControllerBase
    {
        private readonly IFleetModule _fleetModule;

        public FleetController(IFleetModule fleetModule)
        {
            _fleetModule = fleetModule;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleCommand command)
        {
            var vehicleId = await _fleetModule.CreateVehicle(command.LicensePlate, command.Model, command.Year);
            return CreatedAtAction(nameof(Create), new { id = vehicleId }, null);
        }
    }
}
