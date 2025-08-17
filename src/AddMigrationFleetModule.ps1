param(
    [string]$Name = $("Mig_" + (Get-Date -Format "yyyyMMddHHmmss"))
)

dotnet ef migrations add $Name `
  --context FleetDbContext `
  --project Modules/Fleet/VCheck.Modules.Fleet/VCheck.Modules.Fleet.csproj `
  --startup-project API/VCheck.Api.csproj