using FestivalIncidencias.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FestivalIncidencias.Api.Controllers;

[ApiController]
[Route("api/equipos")]
public sealed class EquiposController : ControllerBase
{
    private readonly IIncidenciasService _service;

    public EquiposController(IIncidenciasService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult ObtenerTodos([FromQuery] string? especialidad = null)
    {
        try
        {
            return Ok(_service.ObtenerEquipos(especialidad));
        }
        catch (ErrorIncidenciasException ex)
        {
            return ex.Tipo == TipoErrorIncidencias.Validacion
                ? BadRequest(new { error = ex.Message })
                : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
