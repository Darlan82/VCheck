using VCheck.Modules.Fleet;
using VCheck.Modules.Checklists;

var builder = WebApplication.CreateBuilder(args);

//M�dulos
builder.AddFleetModule();
builder.AddChecklistsModule();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
