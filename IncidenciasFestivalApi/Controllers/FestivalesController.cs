using FestivalIncidencias.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FestivalIncidencias.Api.Controllers;

[ApiController]
[Route("api/festivales")]
public sealed class FestivalesController : ControllerBase
{
    private readonly IIncidenciasService _service;

    public FestivalesController(IIncidenciasService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult ObtenerTodos() => Ok(_service.ObtenerFestivales());

    [HttpGet("{id:int}/resumen")]
    public IActionResult ObtenerResumen(int id)
    {
        try
        {
            return Ok(_service.ObtenerResumenFestival(id));
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
