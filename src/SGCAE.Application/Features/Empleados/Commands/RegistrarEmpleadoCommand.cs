using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Exceptions;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Application.Features.Empleados.Commands;

/// <summary>
/// Comando con el que registro un nuevo empleado en el sistema (RF-01).
/// </summary>
public sealed record RegistrarEmpleadoCommand(
    string Identificacion,
    string Nombres,
    string Apellidos,
    string CorreoElectronico,
    string Cargo,
    string AreaOrganizacional) : ICommand<EmpleadoDto>;

public sealed class RegistrarEmpleadoCommandHandler : ICommandHandler<RegistrarEmpleadoCommand, EmpleadoDto>
{
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarEmpleadoCommandHandler(IEmpleadoRepository empleadoRepository, IUnitOfWork unitOfWork)
    {
        _empleadoRepository = empleadoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmpleadoDto> HandleAsync(RegistrarEmpleadoCommand command, CancellationToken cancellationToken = default)
    {
        var documento = DocumentoIdentidad.Crear(command.Identificacion);
        var correo = CorreoElectronico.Crear(command.CorreoElectronico);

        var existente = await _empleadoRepository.ObtenerPorIdentificacionAsync(documento.Valor, cancellationToken);
        if (existente is not null)
            throw new DomainException($"Ya existe un empleado registrado con la identificación '{documento.Valor}'.");

        var empleado = Empleado.Registrar(
            documento,
            command.Nombres,
            command.Apellidos,
            correo,
            command.Cargo,
            command.AreaOrganizacional);

        await _empleadoRepository.AgregarAsync(empleado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return EmpleadoDto.FromEntity(empleado);
    }
}
