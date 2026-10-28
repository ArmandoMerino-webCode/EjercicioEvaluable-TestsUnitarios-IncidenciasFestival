using System.ComponentModel.DataAnnotations;

namespace FestivalIncidencias.Api.DTOs;

public sealed class CrearIncidenciaRequest
{
    [Range(1, int.MaxValue)]
    public int FestivalId { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Zona { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [StringLength(40)]
    public string EspecialidadRequerida { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Prioridad { get; set; } = string.Empty;
}

public sealed class AsignarIncidenciaRequest
{
    [Range(1, int.MaxValue)]
    public int EquipoId { get; set; }
}

public sealed class ResolverIncidenciaRequest
{
    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Resolucion { get; set; } = string.Empty;
}

public sealed record FestivalDto(int Id, string Nombre, bool Activo);

public sealed record EquipoTecnicoDto(
    int Id,
    string Nombre,
    string Especialidad,
    bool Activo,
    int IncidenciasActivas);

public sealed record IncidenciaDto(
    int Id,
    int FestivalId,
    string Zona,
    string Descripcion,
    string EspecialidadRequerida,
    string Prioridad,
    string Estado,
    DateTimeOffset CreadaUtc,
    int? EquipoId,
    DateTimeOffset? ResueltaUtc,
    string? Resolucion);

public sealed record ResumenFestivalDto(
    int FestivalId,
    string Festival,
    int TotalIncidencias,
    int Abiertas,
    int Asignadas,
    int Resueltas,
    int CriticasPendientes);
