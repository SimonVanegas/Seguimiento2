using Microsoft.AspNetCore.Mvc;
using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Accesos.Commands;
using SGCAE.Application.Features.Accesos.DTOs;
using SGCAE.Application.Features.Accesos.Queries;
using SGCAE.Domain.Enums;

namespace SGCAE.Api.Controllers;

/// <summary>
/// Expongo los endpoints REST para registrar entradas/salidas (RF-03) y consultar el historial paginado (RF-04).
/// </summary>
[ApiController]
[Route("api/accesos")]
[Produces("application/json")]
public class AccesosController : ControllerBase
{
    private readonly ICommandHandler<RegistrarMovimientoAccesoCommand, RegistroAccesoDto> _registrarMovimientoHandler;
    private readonly IQueryHandler<ConsultarHistorialAccesosQuery, PagedResult<RegistroAccesoDto>> _consultarHistorialHandler;

    public AccesosController(
        ICommandHandler<RegistrarMovimientoAccesoCommand, RegistroAccesoDto> registrarMovimientoHandler,
        IQueryHandler<ConsultarHistorialAccesosQuery, PagedResult<RegistroAccesoDto>> consultarHistorialHandler)
    {
        _registrarMovimientoHandler = registrarMovimientoHandler;
        _consultarHistorialHandler = consultarHistorialHandler;
    }

    /// <summary>
    /// Registro una solicitud de entrada o salida validando al empleado y su credencial, incluyendo los accesos rechazados (RF-03).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RegistroAccesoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarMovimiento([FromBody] RegistrarMovimientoAccesoCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _registrarMovimientoHandler.HandleAsync(command, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Consulto el historial paginado de accesos filtrando por empleado, rango de fechas, punto de acceso y resultado (RF-04).
    /// </summary>
    [HttpGet("historial")]
    [ProducesResponseType(typeof(PagedResult<RegistroAccesoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConsultarHistorial(
        [FromQuery] Guid? empleadoId,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin,
        [FromQuery] Guid? puntoAccesoId,
        [FromQuery] ResultadoAcceso? resultado,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new ConsultarHistorialAccesosQuery(
            empleadoId,
            fechaInicio,
            fechaFin,
            puntoAccesoId,
            resultado,
            pageNumber,
            pageSize);

        var respuesta = await _consultarHistorialHandler.HandleAsync(query, cancellationToken);
        return Ok(respuesta);
    }
}
