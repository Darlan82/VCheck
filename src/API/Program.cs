using VCheck.Modules.Fleet;
using VCheck.Modules.Checklists;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Módulos
builder.AddFleetModule();
builder.AddChecklistsModule();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();

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
