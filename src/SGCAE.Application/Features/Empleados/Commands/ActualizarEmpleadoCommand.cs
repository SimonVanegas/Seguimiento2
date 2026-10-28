using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Application.Features.Empleados.Commands;

/// <summary>
/// Comando con el que actualizo los datos de un empleado existente (RF-01).
/// </summary>
public sealed record ActualizarEmpleadoCommand(
    Guid Id,
    string Nombres,
    string Apellidos,
    string CorreoElectronico,
    string Cargo,
    string AreaOrganizacional) : ICommand<EmpleadoDto>;

public sealed class ActualizarEmpleadoCommandHandler : ICommandHandler<ActualizarEmpleadoCommand, EmpleadoDto>
{
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarEmpleadoCommandHandler(IEmpleadoRepository empleadoRepository, IUnitOfWork unitOfWork)
    {
        _empleadoRepository = empleadoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmpleadoDto> HandleAsync(ActualizarEmpleadoCommand command, CancellationToken cancellationToken = default)
    {
        var empleado = await _empleadoRepository.ObtenerPorIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el empleado con ID '{command.Id}'.");

        var correo = CorreoElectronico.Crear(command.CorreoElectronico);

        empleado.ActualizarDatos(
            command.Nombres,
            command.Apellidos,
            correo,
            command.Cargo,
            command.AreaOrganizacional);

        _empleadoRepository.Actualizar(empleado);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return EmpleadoDto.FromEntity(empleado);
    }
}
