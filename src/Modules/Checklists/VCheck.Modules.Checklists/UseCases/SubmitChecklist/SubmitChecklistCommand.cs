namespace VCheck.Modules.Checklists.UseCases.SubmitChecklist;

/// <summary>
/// Comando para submeter um checklist para aprovação.
/// </summary>
/// <param name="RowVersion">Valor de concorrência (rowVersion) do checklist obtido previamente. Deve ser enviado exatamente como retornado pela API (Base64).</param>
public sealed record SubmitChecklistCommand(byte[] RowVersion);
