using FestivalIncidencias.Api.Models;

namespace FestivalIncidencias.Api.Repositories;

public interface IFestivalRepository
{
    IReadOnlyList<Festival> ObtenerFestivales();

    Festival? ObtenerFestival(int id);

    IReadOnlyList<EquipoTecnico> ObtenerEquipos();

    EquipoTecnico? ObtenerEquipo(int id);

    IReadOnlyList<Incidencia> ObtenerIncidenciasPorFestival(int festivalId);

    Incidencia? ObtenerIncidencia(int id);

    int ContarIncidenciasActivasDeEquipo(int equipoId);

    Incidencia CrearIncidencia(Incidencia incidencia);

    void ActualizarIncidencia(Incidencia incidencia);
}
