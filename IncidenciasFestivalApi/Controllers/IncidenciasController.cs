using FestivalIncidencias.Api.DTOs;
using FestivalIncidencias.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FestivalIncidencias.Api.Controllers;

[ApiController]
[Route("api/incidencias")]
public sealed class IncidenciasController : ControllerBase
{
    private readonly IIncidenciasService _service;

    public IncidenciasController(IIncidenciasService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult ObtenerPorFestival([FromQuery] int festivalId)
    {
        try
        {
            return Ok(_service.ObtenerIncidencias(festivalId));
        }
        catch (ErrorIncidenciasException ex)
        {
            return TraducirError(ex);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult ObtenerPorId(int id)
    {
        try
        {
            return Ok(_service.ObtenerIncidencia(id));
        }
        catch (ErrorIncidenciasException ex)
        {
            return TraducirError(ex);
        }
    }

    [HttpPost]
    public IActionResult Crear([FromBody] CrearIncidenciaRequest request)
    {
        try
        {
            var creada = _service.CrearIncidencia(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ErrorIncidenciasException ex)
        {
            return TraducirError(ex);
        }
    }

    [HttpPost("{id:int}/asignar")]
    public IActionResult Asignar(int id, [FromBody] AsignarIncidenciaRequest request)
    {
        try
        {
            return Ok(_service.AsignarIncidencia(id, request));
        }
        catch (ErrorIncidenciasException ex)
        {
            return TraducirError(ex);
        }
    }

    [HttpPost("{id:int}/resolver")]
    public IActionResult Resolver(int id, [FromBody] ResolverIncidenciaRequest request)
    {
        try
        {
            return Ok(_service.ResolverIncidencia(id, request));
        }
        catch (ErrorIncidenciasException ex)
        {
            return TraducirError(ex);
        }
    }

    private IActionResult TraducirError(ErrorIncidenciasException ex) => ex.Tipo switch
    {
        TipoErrorIncidencias.Validacion => BadRequest(new { error = ex.Message }),
        TipoErrorIncidencias.NoEncontrado => NotFound(new { error = ex.Message }),
        TipoErrorIncidencias.Conflicto => Conflict(new { error = ex.Message }),
        _ => StatusCode(StatusCodes.Status500InternalServerError)
    };
}
