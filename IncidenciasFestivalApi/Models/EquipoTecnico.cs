namespace FestivalIncidencias.Api.Models;

public sealed record EquipoTecnico(
    int Id,
    string Nombre,
    EspecialidadTecnica Especialidad,
    bool Activo);
