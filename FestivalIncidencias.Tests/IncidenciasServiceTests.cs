
using FestivalIncidencias.Api.Repositories;
using FestivalIncidencias.Api.Services;
using Xunit;

using FestivalIncidencias.Api.DTOs;

namespace FestivalIncidencias.Tests;

public class IncidenciasServiceTests
{
    [Fact]
    public void ObtenerFestivales_DebeDevolverDosFestivales()
    {
        // ARRANGE: preparamos los objetos
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        // ACT: ejecutamos el método
        var resultado = servicio.ObtenerFestivales();

        // ASSERT: comprobamos el resultado
        Assert.Equal(2, resultado.Count);
    }


    [Fact]
    public void ObtenerIncidencias_FestivalInexistente_LanzaError()
    {
        // ARRANGE
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        // ACT y ASSERT
        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.ObtenerIncidencias(999)
        );

        Assert.Equal(
            TipoErrorIncidencias.NoEncontrado,
            excepcion.Tipo
        );
    }


    [Fact]
    public void CrearIncidencia_DatosValidos_CreaIncidenciaAbierta()
    {
        // ARRANGE
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitud = new CrearIncidenciaRequest
        {
            FestivalId = 1,
            Zona = "Escenario principal",
            Descripcion = "Fallo en el sistema de sonido",
            EspecialidadRequerida = "Sonido",
            Prioridad = "Alta"
        };

        // ACT
        var resultado = servicio.CrearIncidencia(solicitud);

        // ASSERT
        Assert.True(resultado.Id > 0);
        Assert.Equal(1, resultado.FestivalId);
        Assert.Equal("Escenario principal", resultado.Zona);
        Assert.Equal("Alta", resultado.Prioridad);
        Assert.Equal("Abierta", resultado.Estado);
    }


    [Fact]
    public void CrearIncidencia_FestivalInactivo_LanzaConflicto()
    {
        // ARRANGE
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitud = new CrearIncidenciaRequest
        {
            FestivalId = 2,
            Zona = "Escenario principal",
            Descripcion = "Fallo en el sistema de sonido",
            EspecialidadRequerida = "Sonido",
            Prioridad = "Alta"
        };

        // ACT
        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.CrearIncidencia(solicitud)
        );

        // ASSERT
        Assert.Equal(TipoErrorIncidencias.Conflicto, excepcion.Tipo);
    }


    [Fact]
    public void CrearIncidencia_ZonaDemasiadoCorta_LanzaErrorValidacion()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitud = new CrearIncidenciaRequest
        {
            FestivalId = 1,
            Zona = "A",
            Descripcion = "Fallo en el sistema de sonido",
            EspecialidadRequerida = "Sonido",
            Prioridad = "Alta"
        };

        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.CrearIncidencia(solicitud)
        );

        Assert.Equal(TipoErrorIncidencias.Validacion, excepcion.Tipo);
    }


    [Fact]
    public void CrearIncidencia_DescripcionDemasiadoCorta_LanzaErrorValidacion()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitud = new CrearIncidenciaRequest
        {
            FestivalId = 1,
            Zona = "Escenario principal",
            Descripcion = "Fallo",
            EspecialidadRequerida = "Sonido",
            Prioridad = "Alta"
        };

        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.CrearIncidencia(solicitud)
        );

        Assert.Equal(TipoErrorIncidencias.Validacion, excepcion.Tipo);
    }


    [Fact]
    public void CrearIncidencia_EspecialidadNoValida_LanzaErrorValidacion()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitud = new CrearIncidenciaRequest
        {
            FestivalId = 1,
            Zona = "Escenario principal",
            Descripcion = "Fallo en el sistema de sonido",
            EspecialidadRequerida = "EspecialidadInventada",
            Prioridad = "Alta"
        };

        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.CrearIncidencia(solicitud)
        );

        Assert.Equal(TipoErrorIncidencias.Validacion, excepcion.Tipo);
    }


    [Fact]
    public void AsignarIncidencia_IncidenciaInexistente_LanzaError()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitud = new AsignarIncidenciaRequest
        {
            EquipoId = 1
        };

        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.AsignarIncidencia(999, solicitud)
        );

        Assert.Equal(TipoErrorIncidencias.NoEncontrado, excepcion.Tipo);
    }


    [Fact]
    public void ResolverIncidencia_NoAsignada_LanzaConflicto()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitudCreacion = new CrearIncidenciaRequest
        {
            FestivalId = 1,
            Zona = "Escenario principal",
            Descripcion = "Fallo en el sistema de sonido",
            EspecialidadRequerida = "Sonido",
            Prioridad = "Alta"
        };

        var incidencia = servicio.CrearIncidencia(solicitudCreacion);

        var solicitudResolucion = new ResolverIncidenciaRequest
        {
            Resolucion = "Se ha revisado el equipo"
        };

        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.ResolverIncidencia(
                incidencia.Id, solicitudResolucion)
        );

        Assert.Equal(TipoErrorIncidencias.Conflicto, excepcion.Tipo);
    }


    [Fact]
    public void ResolverIncidencia_ResolucionDemasiadoCorta_LanzaErrorValidacion()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var solicitudCreacion = new CrearIncidenciaRequest
        {
            FestivalId = 1,
            Zona = "Escenario principal",
            Descripcion = "Fallo en el sistema de sonido",
            EspecialidadRequerida = "Sonido",
            Prioridad = "Alta"
        };

        var incidencia = servicio.CrearIncidencia(solicitudCreacion);

        var equipo = servicio.ObtenerEquipos("Sonido")
            .First(e => e.Activo && e.IncidenciasActivas < 3);

        servicio.AsignarIncidencia(
            incidencia.Id,
            new AsignarIncidenciaRequest
            {
                EquipoId = equipo.Id
            }
        );

        var solicitudResolucion = new ResolverIncidenciaRequest
        {
            Resolucion = "Fallo"
        };

        var excepcion = Assert.Throws<ErrorIncidenciasException>(
            () => servicio.ResolverIncidencia(
                incidencia.Id, solicitudResolucion)
        );

        Assert.Equal(TipoErrorIncidencias.Validacion, excepcion.Tipo);
    }


    [Fact]
    public void ObtenerResumenFestival_FestivalExistente_DevuelveResumenCorrecto()
    {
        var repositorio = new FestivalRepositoryMemoria();
        var servicio = new IncidenciasService(repositorio);

        var resultado = servicio.ObtenerResumenFestival(1);

        Assert.Equal(1, resultado.FestivalId);
        Assert.False(string.IsNullOrWhiteSpace(resultado.Festival));
        Assert.Equal(
            resultado.TotalIncidencias,
            resultado.Abiertas + resultado.Asignadas + resultado.Resueltas
        );
    }




}
