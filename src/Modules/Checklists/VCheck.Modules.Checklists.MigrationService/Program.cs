using VCheck.Modules.Checklists.MigrationService;
using VCheck.Modules.Checklists;

var builder = Host.CreateApplicationBuilder(args);

builder.AddChecklistsDbContext();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
