using FestivalIncidencias.Api.Models;

namespace FestivalIncidencias.Api.Repositories;

public sealed class FestivalRepositoryMemoria : IFestivalRepository
{
    private readonly List<Festival> _festivales =
    [
        new Festival(1, "Festival Ciudad Sonora", true),
        new Festival(2, "Festival Nocturno del Valle", false)
    ];

    private readonly List<EquipoTecnico> _equipos =
    [
        new EquipoTecnico(1, "Equipo Chispa", EspecialidadTecnica.Electricidad, true),
        new EquipoTecnico(2, "Equipo Acústica", EspecialidadTecnica.Sonido, true),
        new EquipoTecnico(3, "Equipo Brillo", EspecialidadTecnica.Limpieza, true),
        new EquipoTecnico(4, "Equipo Control", EspecialidadTecnica.Seguridad, false),
        new EquipoTecnico(5, "Equipo Voltio", EspecialidadTecnica.Electricidad, true),
        new EquipoTecnico(6, "Equipo Acceso", EspecialidadTecnica.Accesos, true),
        new EquipoTecnico(7, "Equipo Escena", EspecialidadTecnica.Escenario, true)
    ];

    private readonly List<Incidencia> _incidencias = [];
    private int _siguienteId = 1;

    public IReadOnlyList<Festival> ObtenerFestivales() => _festivales.ToList();

    public Festival? ObtenerFestival(int id) =>
        _festivales.FirstOrDefault(f => f.Id == id);

    public IReadOnlyList<EquipoTecnico> ObtenerEquipos() => _equipos.ToList();

    public EquipoTecnico? ObtenerEquipo(int id) =>
        _equipos.FirstOrDefault(e => e.Id == id);

    public IReadOnlyList<Incidencia> ObtenerIncidenciasPorFestival(int festivalId) =>
        _incidencias
            .Where(i => i.FestivalId == festivalId)
            .Select(Copiar)
            .ToList();

    public Incidencia? ObtenerIncidencia(int id)
    {
        var incidencia = _incidencias.FirstOrDefault(i => i.Id == id);
        return incidencia is null ? null : Copiar(incidencia);
    }

    public int ContarIncidenciasActivasDeEquipo(int equipoId) =>
        _incidencias.Count(i =>
            i.EquipoId == equipoId && i.Estado != EstadoIncidencia.Resuelta);

    public Incidencia CrearIncidencia(Incidencia incidencia)
    {
        var guardada = Copiar(incidencia);
        guardada.Id = _siguienteId++;
        _incidencias.Add(guardada);
        return Copiar(guardada);
    }

    public void ActualizarIncidencia(Incidencia incidencia)
    {
        var indice = _incidencias.FindIndex(i => i.Id == incidencia.Id);
        if (indice < 0)
        {
            throw new InvalidOperationException(
                $"No existe la incidencia {incidencia.Id}.");
        }

        _incidencias[indice] = Copiar(incidencia);
    }

    private static Incidencia Copiar(Incidencia origen) => new()
    {
        Id = origen.Id,
        FestivalId = origen.FestivalId,
        Zona = origen.Zona,
        Descripcion = origen.Descripcion,
        EspecialidadRequerida = origen.EspecialidadRequerida,
        Prioridad = origen.Prioridad,
        Estado = origen.Estado,
        CreadaUtc = origen.CreadaUtc,
        EquipoId = origen.EquipoId,
        ResueltaUtc = origen.ResueltaUtc,
        Resolucion = origen.Resolucion
    };
}
