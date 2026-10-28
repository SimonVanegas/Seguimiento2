using Microsoft.AspNetCore.Mvc;
using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.PuntosAcceso.Commands;
using SGCAE.Application.Features.PuntosAcceso.DTOs;
using SGCAE.Application.Features.PuntosAcceso.Queries;

namespace SGCAE.Api.Controllers;

/// <summary>
/// Expongo los endpoints REST para administrar los puntos de acceso físicos de la organización (RF-06).
/// </summary>
[ApiController]
[Route("api/puntos-acceso")]
[Produces("application/json")]
public class PuntosAccesoController : ControllerBase
{
    private readonly ICommandHandler<RegistrarPuntoAccesoCommand, PuntoAccesoDto> _registrarHandler;
    private readonly IQueryHandler<ListarPuntosAccesoQuery, IReadOnlyList<PuntoAccesoDto>> _listarHandler;

    public PuntosAccesoController(
        ICommandHandler<RegistrarPuntoAccesoCommand, PuntoAccesoDto> registrarHandler,
        IQueryHandler<ListarPuntosAccesoQuery, IReadOnlyList<PuntoAccesoDto>> listarHandler)
    {
        _registrarHandler = registrarHandler;
        _listarHandler = listarHandler;
    }

    /// <summary>
    /// Registro un nuevo punto de acceso físico como recepción, parqueadero, bodega o área restringida (RF-06).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PuntoAccesoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarPuntoAccesoCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _registrarHandler.HandleAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    /// <summary>
    /// Consulto los puntos de acceso físicos identificando cuáles se encuentran habilitados (RF-06).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PuntoAccesoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] bool? soloHabilitados, CancellationToken cancellationToken)
    {
        var resultado = await _listarHandler.HandleAsync(new ListarPuntosAccesoQuery(soloHabilitados), cancellationToken);
        return Ok(resultado);
    }
}
