using FestivalIncidencias.Api.Repositories;
using FestivalIncidencias.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFestivalRepository, FestivalRepositoryMemoria>();
builder.Services.AddScoped<IIncidenciasService, IncidenciasService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

// Necesario para que WebApplicationFactory pueda localizar el punto de entrada
// cuando se creen pruebas de integración.
public partial class Program { }
