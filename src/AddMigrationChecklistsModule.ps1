param(
    [string]$Name = $("Mig_" + (Get-Date -Format "yyyyMMddHHmmss"))
)

dotnet ef migrations add $Name `
  --context ChecklistsDbContext `
  --project Modules/Checklists/VCheck.Modules.Checklists/VCheck.Modules.Checklists.csproj `
  --startup-project API/VCheck.Api.csproj