using Microsoft.AspNetCore.Mvc;
using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Empleados.Commands;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Application.Features.Empleados.Queries;

namespace SGCAE.Api.Controllers;

/// <summary>
/// Expongo los endpoints REST para gestionar la información de los empleados (RF-01).
/// </summary>
[ApiController]
[Route("api/empleados")]
[Produces("application/json")]
public class EmpleadosController : ControllerBase
{
    private readonly ICommandHandler<RegistrarEmpleadoCommand, EmpleadoDto> _registrarHandler;
    private readonly ICommandHandler<ActualizarEmpleadoCommand, EmpleadoDto> _actualizarHandler;
    private readonly ICommandHandler<DesactivarEmpleadoCommand, EmpleadoDto> _desactivarHandler;
    private readonly IQueryHandler<ObtenerEmpleadoPorIdQuery, EmpleadoDto> _obtenerPorIdHandler;
    private readonly IQueryHandler<ListarEmpleadosQuery, IReadOnlyList<EmpleadoDto>> _listarHandler;

    public EmpleadosController(
        ICommandHandler<RegistrarEmpleadoCommand, EmpleadoDto> registrarHandler,
        ICommandHandler<ActualizarEmpleadoCommand, EmpleadoDto> actualizarHandler,
        ICommandHandler<DesactivarEmpleadoCommand, EmpleadoDto> desactivarHandler,
        IQueryHandler<ObtenerEmpleadoPorIdQuery, EmpleadoDto> obtenerPorIdHandler,
        IQueryHandler<ListarEmpleadosQuery, IReadOnlyList<EmpleadoDto>> listarHandler)
    {
        _registrarHandler = registrarHandler;
        _actualizarHandler = actualizarHandler;
        _desactivarHandler = desactivarHandler;
        _obtenerPorIdHandler = obtenerPorIdHandler;
        _listarHandler = listarHandler;
    }

    /// <summary>
    /// Registro un nuevo empleado en el sistema (RF-01).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarEmpleadoCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _registrarHandler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Id }, resultado);
    }

    /// <summary>
    /// Consulto el listado completo de empleados registrados (RF-01).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmpleadoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var resultado = await _listarHandler.HandleAsync(new ListarEmpleadosQuery(), cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Consulto la información de un empleado por su ID (RF-01).
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerPorIdHandler.HandleAsync(new ObtenerEmpleadoPorIdQuery(id), cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Actualizo la información de un empleado existente (RF-01).
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarEmpleadoRequest request, CancellationToken cancellationToken)
    {
        var command = new ActualizarEmpleadoCommand(
            id,
            request.Nombres,
            request.Apellidos,
            request.CorreoElectronico,
            request.Cargo,
            request.AreaOrganizacional);

        var resultado = await _actualizarHandler.HandleAsync(command, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Desactivo el vínculo laboral de un empleado (RF-01).
    /// </summary>
    [HttpPatch("{id:guid}/desactivar")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desactivar(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _desactivarHandler.HandleAsync(new DesactivarEmpleadoCommand(id), cancellationToken);
        return Ok(resultado);
    }
}

public sealed record ActualizarEmpleadoRequest(
    string Nombres,
    string Apellidos,
    string CorreoElectronico,
    string Cargo,
    string AreaOrganizacional);
