using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Credenciales.DTOs;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Exceptions;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Application.Features.Credenciales.Commands;

/// <summary>
/// Comando con el que asigno una credencial de acceso a un empleado activo (RF-02).
/// </summary>
public sealed record AsignarCredencialCommand(
    Guid EmpleadoId,
    string Codigo) : ICommand<CredencialDto>;

public sealed class AsignarCredencialCommandHandler : ICommandHandler<AsignarCredencialCommand, CredencialDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AsignarCredencialCommandHandler(
        ICredencialRepository credencialRepository,
        IEmpleadoRepository empleadoRepository,
        IUnitOfWork unitOfWork)
    {
        _credencialRepository = credencialRepository;
        _empleadoRepository = empleadoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CredencialDto> HandleAsync(AsignarCredencialCommand command, CancellationToken cancellationToken = default)
    {
        var codigo = CodigoCredencial.Crear(command.Codigo);

        var credencialExistente = await _credencialRepository.ObtenerPorCodigoAsync(codigo.Valor, cancellationToken);
        if (credencialExistente is not null)
            throw new DomainException($"Ya existe una credencial registrada con el código '{codigo.Valor}'.");

        var empleado = await _empleadoRepository.ObtenerPorIdAsync(command.EmpleadoId, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el empleado con ID '{command.EmpleadoId}'.");

        var credencial = Credencial.Asignar(codigo, empleado);

        await _credencialRepository.AgregarAsync(credencial, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CredencialDto.FromEntity(credencial);
    }
}
