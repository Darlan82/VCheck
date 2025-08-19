using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using VCheck.Modules.Checklists.UseCases.StartChecklist;
using VCheck.Modules.Checklists.UseCases.UpdateChecklistItem;
using VCheck.SharedKernel;

namespace VCheck.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [SwaggerTag("Fluxos de execução e aprovação de checklists de veículos.")]
    public class ChecklistsController : ControllerBase
    {
        private readonly IChecklistsModule _checklistsModule;

        public ChecklistsController(IChecklistsModule checklistsModule)
        {
            _checklistsModule = checklistsModule;
        }

        [HttpPost]
        [Authorize(Roles = "executor")]
        [SwaggerOperation(
            Summary = "Inicia um novo checklist",
            Description = "Fluxo: Iniciar checklist (executor). Regras: apenas role executor; impede segundo checklist em andamento para o mesmo veículo ou concorrência de executores.",
            OperationId = "StartChecklist")]
        [SwaggerResponse(201, "Checklist iniciado.")]
        [SwaggerResponse(400, "Falha de validação ou já existe checklist em andamento.")]
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
        [SwaggerOperation(
            Summary = "Atualiza item do checklist",
            Description = "Fluxo: Atualizar item (executor). Regras: somente executor dono; exige RowVersion; proibido após submissão.",
            OperationId = "UpdateChecklistItem")]
        [SwaggerResponse(204, "Item atualizado.")]
        [SwaggerResponse(400, "Erro de negócio, concorrência ou estado inválido.")]
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
        [SwaggerOperation(
            Summary = "Submete checklist para aprovação",
            Description = "Fluxo: Submeter (executor). Regras: somente executor dono; exige RowVersion; bloqueia novas alterações de itens.",
            OperationId = "SubmitChecklist")]
        [SwaggerResponse(204, "Checklist submetido.")]
        [SwaggerResponse(400, "Erro de negócio, concorrência ou estado inválido.")]
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
        [SwaggerOperation(
            Summary = "Aprova checklist submetido",
            Description = "Fluxo: Aprovar checklist (supervisor). Regras: somente role supervisor; estado deve ser Submetido.",
            OperationId = "ApproveChecklist")]
        [SwaggerResponse(204, "Checklist aprovado.")]
        [SwaggerResponse(400, "Checklist não apto ou outro erro de negócio.")]
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
