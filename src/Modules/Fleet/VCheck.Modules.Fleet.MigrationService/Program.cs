using VCheck.Modules.Fleet;
using VCheck.Modules.Fleet.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddFleetDbContext();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
