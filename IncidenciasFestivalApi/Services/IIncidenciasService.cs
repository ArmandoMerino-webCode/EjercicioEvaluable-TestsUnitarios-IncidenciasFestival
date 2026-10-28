using FestivalIncidencias.Api.DTOs;
using FestivalIncidencias.Api.Models;

namespace FestivalIncidencias.Api.Services;

public interface IIncidenciasService
{
    IReadOnlyList<FestivalDto> ObtenerFestivales();

    IReadOnlyList<EquipoTecnicoDto> ObtenerEquipos(string? especialidad);

    IReadOnlyList<IncidenciaDto> ObtenerIncidencias(int festivalId);

    IncidenciaDto ObtenerIncidencia(int id);

    IncidenciaDto CrearIncidencia(CrearIncidenciaRequest solicitud);

    IncidenciaDto AsignarIncidencia(int id, AsignarIncidenciaRequest solicitud);

    IncidenciaDto ResolverIncidencia(int id, ResolverIncidenciaRequest solicitud);

    ResumenFestivalDto ObtenerResumenFestival(int festivalId);
}
