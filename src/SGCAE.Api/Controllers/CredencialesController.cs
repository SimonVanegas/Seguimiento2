using Microsoft.AspNetCore.Mvc;
using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Credenciales.Commands;
using SGCAE.Application.Features.Credenciales.DTOs;
using SGCAE.Application.Features.Credenciales.Queries;

namespace SGCAE.Api.Controllers;

/// <summary>
/// Expongo los endpoints REST para controlar las credenciales de los empleados (RF-02).
/// </summary>
[ApiController]
[Route("api/credenciales")]
[Produces("application/json")]
public class CredencialesController : ControllerBase
{
    private readonly ICommandHandler<AsignarCredencialCommand, CredencialDto> _asignarHandler;
    private readonly ICommandHandler<BloquearCredencialCommand, CredencialDto> _bloquearHandler;
    private readonly ICommandHandler<RevocarCredencialCommand, CredencialDto> _revocarHandler;
    private readonly IQueryHandler<ConsultarEstadoCredencialQuery, CredencialDto> _consultarEstadoHandler;

    public CredencialesController(
        ICommandHandler<AsignarCredencialCommand, CredencialDto> asignarHandler,
        ICommandHandler<BloquearCredencialCommand, CredencialDto> bloquearHandler,
        ICommandHandler<RevocarCredencialCommand, CredencialDto> revocarHandler,
        IQueryHandler<ConsultarEstadoCredencialQuery, CredencialDto> consultarEstadoHandler)
    {
        _asignarHandler = asignarHandler;
        _bloquearHandler = bloquearHandler;
        _revocarHandler = revocarHandler;
        _consultarEstadoHandler = consultarEstadoHandler;
    }

    /// <summary>
    /// Asigno una nueva credencial de acceso a un empleado activo (RF-02).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CredencialDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Asignar([FromBody] AsignarCredencialCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _asignarHandler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(ConsultarEstado), new { codigo = resultado.Codigo }, resultado);
    }

    /// <summary>
    /// Consulto el estado de una credencial por su código único y determino si puede utilizarse (RF-02).
    /// </summary>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(CredencialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConsultarEstado(string codigo, CancellationToken cancellationToken)
    {
        var resultado = await _consultarEstadoHandler.HandleAsync(new ConsultarEstadoCredencialQuery(codigo), cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Bloqueo una credencial de acceso vigente (RF-02).
    /// </summary>
    [HttpPatch("{codigo}/bloquear")]
    [ProducesResponseType(typeof(CredencialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Bloquear(string codigo, CancellationToken cancellationToken)
    {
        var resultado = await _bloquearHandler.HandleAsync(new BloquearCredencialCommand(codigo), cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Revoco definitivamente una credencial de acceso (RF-02).
    /// </summary>
    [HttpPatch("{codigo}/revocar")]
    [ProducesResponseType(typeof(CredencialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revocar(string codigo, CancellationToken cancellationToken)
    {
        var resultado = await _revocarHandler.HandleAsync(new RevocarCredencialCommand(codigo), cancellationToken);
        return Ok(resultado);
    }
}
