using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VCheck.SharedKernel;
using VCheck.Modules.Checklists.UseCases.StartChecklist;
using VCheck.Modules.Checklists.UseCases.UpdateChecklistItem;

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
            var checklistId = await _checklistsModule.StartNewChecklist(command.VehicleId, userId);
            return CreatedAtAction(nameof(Start), new { id = checklistId }, null);
        }

        [HttpPut("{checklistId}/items/{itemId}")]
        [Authorize(Roles = "executor")]
        public async Task<IActionResult> UpdateItem(Guid checklistId, Guid itemId, [FromBody] UpdateChecklistItemCommand command)
        {
            var userId = GetUserId();
            await _checklistsModule.UpdateChecklistItem(checklistId, itemId, command.Status, command.Observations, command.RowVersion, userId);
            return NoContent();
        }

        [HttpPost("{checklistId}/submit")]
        [Authorize(Roles = "executor")]
        public async Task<IActionResult> Submit(Guid checklistId, [FromBody] byte[] rowVersion)
        {
            var userId = GetUserId();
            await _checklistsModule.SubmitForApproval(checklistId, rowVersion, userId);
            return NoContent();
        }

        [HttpPost("{checklistId}/approve")]
        [Authorize(Roles = "supervisor")]
        public async Task<IActionResult> Approve(Guid checklistId)
        {
            var supervisorId = GetUserId();
            await _checklistsModule.ApproveChecklist(checklistId, supervisorId);
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
