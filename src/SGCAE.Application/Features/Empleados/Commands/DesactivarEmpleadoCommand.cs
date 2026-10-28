using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.Empleados.Commands;

/// <summary>
/// Comando con el que desactivo el vínculo laboral de un empleado (RF-01).
/// </summary>
public sealed record DesactivarEmpleadoCommand(Guid Id) : ICommand<EmpleadoDto>;

public sealed class DesactivarEmpleadoCommandHandler : ICommandHandler<DesactivarEmpleadoCommand, EmpleadoDto>
{
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DesactivarEmpleadoCommandHandler(IEmpleadoRepository empleadoRepository, IUnitOfWork unitOfWork)
    {
        _empleadoRepository = empleadoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmpleadoDto> HandleAsync(DesactivarEmpleadoCommand command, CancellationToken cancellationToken = default)
    {
        var empleado = await _empleadoRepository.ObtenerPorIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el empleado con ID '{command.Id}'.");

        empleado.Desactivar();

        _empleadoRepository.Actualizar(empleado);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return EmpleadoDto.FromEntity(empleado);
    }
}
