using FestivalIncidencias.Api.DTOs;
using FestivalIncidencias.Api.Models;
using FestivalIncidencias.Api.Repositories;

namespace FestivalIncidencias.Api.Services;

public sealed class IncidenciasService : IIncidenciasService
{
    private const int MaxIncidenciasActivasPorEquipo = 3;
    private readonly IFestivalRepository _repositorio;

    public IncidenciasService(IFestivalRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public IReadOnlyList<FestivalDto> ObtenerFestivales() =>
        _repositorio.ObtenerFestivales()
            .Select(f => new FestivalDto(f.Id, f.Nombre, f.Activo))
            .ToList();

    public IReadOnlyList<EquipoTecnicoDto> ObtenerEquipos(string? especialidad)
    {
        EspecialidadTecnica? filtro = null;
        if (!string.IsNullOrWhiteSpace(especialidad))
        {
            filtro = ParsearEnum<EspecialidadTecnica>(
                especialidad, "especialidad");
        }

        return _repositorio.ObtenerEquipos()
            .Where(e => !filtro.HasValue || e.Especialidad == filtro.Value)
            .Select(e => new EquipoTecnicoDto(
                e.Id,
                e.Nombre,
                e.Especialidad.ToString(),
                e.Activo,
                _repositorio.ContarIncidenciasActivasDeEquipo(e.Id)))
            .ToList();
    }

    public IReadOnlyList<IncidenciaDto> ObtenerIncidencias(int festivalId)
    {
        _ = ObtenerFestivalExistente(festivalId);
        return _repositorio.ObtenerIncidenciasPorFestival(festivalId)
            .Select(Mapear)
            .ToList();
    }

    public IncidenciaDto ObtenerIncidencia(int id) =>
        Mapear(ObtenerIncidenciaExistente(id));

    public IncidenciaDto CrearIncidencia(CrearIncidenciaRequest solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);

        var festival = ObtenerFestivalExistente(solicitud.FestivalId);
        if (!festival.Activo)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Conflicto,
                "No se pueden registrar incidencias en un festival inactivo.");
        }

        string zona = solicitud.Zona?.Trim() ?? string.Empty;
        if (zona.Length is < 2 or > 80)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Validacion,
                "La zona debe tener entre 2 y 80 caracteres.");
        }

        string descripcion = solicitud.Descripcion?.Trim() ?? string.Empty;
        if (descripcion.Length is < 10 or > 500)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Validacion,
                "La descripción debe tener entre 10 y 500 caracteres.");
        }

        var especialidad = ParsearEnum<EspecialidadTecnica>(
            solicitud.EspecialidadRequerida, "especialidad");
        var prioridad = ParsearEnum<PrioridadIncidencia>(
            solicitud.Prioridad, "prioridad");

        var incidencia = new Incidencia
        {
            FestivalId = festival.Id,
            Zona = zona,
            Descripcion = descripcion,
            EspecialidadRequerida = especialidad,
            Prioridad = prioridad,
            Estado = EstadoIncidencia.Abierta,
            CreadaUtc = DateTimeOffset.UtcNow
        };

        return Mapear(_repositorio.CrearIncidencia(incidencia));
    }

    public IncidenciaDto AsignarIncidencia(
        int id,
        AsignarIncidenciaRequest solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);
        var incidencia = ObtenerIncidenciaExistente(id);

        if (incidencia.Estado != EstadoIncidencia.Abierta)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Conflicto,
                "Solo se pueden asignar incidencias abiertas.");
        }

        var equipo = _repositorio.ObtenerEquipo(solicitud.EquipoId)
            ?? throw new ErrorIncidenciasException(
                TipoErrorIncidencias.NoEncontrado,
                "El equipo técnico indicado no existe.");

        if (!equipo.Activo)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Conflicto,
                "No se pueden asignar incidencias a un equipo inactivo.");
        }

        if (equipo.Especialidad != incidencia.EspecialidadRequerida)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Conflicto,
                "La especialidad del equipo no coincide con la incidencia.");
        }

        if (_repositorio.ContarIncidenciasActivasDeEquipo(equipo.Id)
            >= MaxIncidenciasActivasPorEquipo)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Conflicto,
                "El equipo ya tiene el máximo de incidencias activas.");
        }

        incidencia.EquipoId = equipo.Id;
        incidencia.Estado = EstadoIncidencia.Asignada;
        _repositorio.ActualizarIncidencia(incidencia);

        return Mapear(incidencia);
    }

    public IncidenciaDto ResolverIncidencia(
        int id,
        ResolverIncidenciaRequest solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);
        var incidencia = ObtenerIncidenciaExistente(id);

        if (incidencia.Estado != EstadoIncidencia.Asignada)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Conflicto,
                "Solo se pueden resolver incidencias que estén asignadas.");
        }

        string resolucion = solicitud.Resolucion?.Trim() ?? string.Empty;
        if (resolucion.Length is < 10 or > 500)
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Validacion,
                "La resolución debe tener entre 10 y 500 caracteres.");
        }

        incidencia.Resolucion = resolucion;
        incidencia.Estado = EstadoIncidencia.Resuelta;
        incidencia.ResueltaUtc = DateTimeOffset.UtcNow;
        _repositorio.ActualizarIncidencia(incidencia);

        return Mapear(incidencia);
    }

    public ResumenFestivalDto ObtenerResumenFestival(int festivalId)
    {
        var festival = ObtenerFestivalExistente(festivalId);
        var incidencias = _repositorio.ObtenerIncidenciasPorFestival(festivalId);

        return new ResumenFestivalDto(
            festival.Id,
            festival.Nombre,
            incidencias.Count,
            incidencias.Count(i => i.Estado == EstadoIncidencia.Abierta),
            incidencias.Count(i => i.Estado == EstadoIncidencia.Asignada),
            incidencias.Count(i => i.Estado == EstadoIncidencia.Resuelta),
            incidencias.Count(i => i.Prioridad == PrioridadIncidencia.Critica
                && i.Estado != EstadoIncidencia.Resuelta));
    }

    private Festival ObtenerFestivalExistente(int id) =>
        _repositorio.ObtenerFestival(id)
        ?? throw new ErrorIncidenciasException(
            TipoErrorIncidencias.NoEncontrado,
            "El festival indicado no existe.");

    private Incidencia ObtenerIncidenciaExistente(int id) =>
        _repositorio.ObtenerIncidencia(id)
        ?? throw new ErrorIncidenciasException(
            TipoErrorIncidencias.NoEncontrado,
            "La incidencia indicada no existe.");

    private static TEnum ParsearEnum<TEnum>(string? valor, string nombre)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(valor)
            || !Enum.TryParse<TEnum>(valor.Trim(), true, out var resultado)
            || !Enum.IsDefined(typeof(TEnum), resultado))
        {
            throw new ErrorIncidenciasException(
                TipoErrorIncidencias.Validacion,
                $"El valor de {nombre} no es válido.");
        }

        return resultado;
    }

    private static IncidenciaDto Mapear(Incidencia i) => new(
        i.Id,
        i.FestivalId,
        i.Zona,
        i.Descripcion,
        i.EspecialidadRequerida.ToString(),
        i.Prioridad.ToString(),
        i.Estado.ToString(),
        i.CreadaUtc,
        i.EquipoId,
        i.ResueltaUtc,
        i.Resolucion);
}
