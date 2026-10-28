namespace FestivalIncidencias.Api.Models;

public sealed class Incidencia
{
    public int Id { get; set; }

    public int FestivalId { get; set; }

    public string Zona { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public EspecialidadTecnica EspecialidadRequerida { get; set; }

    public PrioridadIncidencia Prioridad { get; set; }

    public EstadoIncidencia Estado { get; set; } = EstadoIncidencia.Abierta;

    public DateTimeOffset CreadaUtc { get; set; }

    public int? EquipoId { get; set; }

    public DateTimeOffset? ResueltaUtc { get; set; }

    public string? Resolucion { get; set; }
}
