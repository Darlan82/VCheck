using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VCheck.Modules.Checklists.UseCases.StartChecklist;
using VCheck.Modules.Checklists.UseCases.UpdateChecklistItem;
using VCheck.SharedKernel;

namespace VCheck.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ChecklistsController : ControllerBase
    {
        private readonly IChecklistsModule _checklistsModule;

        public ChecklistsController(IChecklistsModule checklistsModule)
        {
            _checklistsModule = checklistsModule;
        }

        [HttpPost]
        [Authorize(Roles = "executor")]
        public async Task<IActionResult> Start([FromBody] StartChecklistCommand command)
        {
            var userId = GetUserId();
            var result = await _checklistsModule.StartNewChecklist(command.VehicleId, userId);
            if (result.Item2 != Error.None)
                return BadRequest(new { error = result.Item2.Code, message = result.Item2.Description });

            return CreatedAtAction(nameof(Start), new { id = result.Item1 }, null);
        }

        [HttpPut("{checklistId}/items/{itemId}")]
        [Authorize(Roles = "executor")]
        public async Task<IActionResult> UpdateItem(Guid checklistId, Guid itemId, [FromBody] UpdateChecklistItemCommand command)
        {
            var userId = GetUserId();
            var result = await _checklistsModule.UpdateChecklistItem(checklistId, itemId, command.Status, command.Observations, command.RowVersion, userId);
            if (result != Error.None)
                return BadRequest(new { error = result.Code, message = result.Description });

            return NoContent();
        }

        [HttpPost("{checklistId}/submit")]
        [Authorize(Roles = "executor")]
        public async Task<IActionResult> Submit(Guid checklistId, [FromBody] byte[] rowVersion)
        {
            var userId = GetUserId();
            var result = await _checklistsModule.SubmitForApproval(checklistId, rowVersion, userId);
            if (result != Error.None)
                return BadRequest(new { error = result.Code, message = result.Description });

            return NoContent();
        }

        [HttpPost("{checklistId}/approve")]
        [Authorize(Roles = "supervisor")]
        public async Task<IActionResult> Approve(Guid checklistId)
        {
            var supervisorId = GetUserId();
            var result = await _checklistsModule.ApproveChecklist(checklistId, supervisorId);
            if (result != Error.None)
                return BadRequest(new { error = result.Code, message = result.Description });

            return NoContent();
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Usuário não autenticado ou claim inválida.");
            return userId;
        }
    }
}
