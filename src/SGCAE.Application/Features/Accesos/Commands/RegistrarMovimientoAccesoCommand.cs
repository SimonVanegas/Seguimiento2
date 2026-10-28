using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Accesos.DTOs;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Enums;
using SGCAE.Domain.Exceptions;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.Accesos.Commands;

/// <summary>
/// Comando con el que proceso una solicitud de entrada o salida, valido las condiciones y registro tanto accesos autorizados como rechazados (RF-03).
/// </summary>
public sealed record RegistrarMovimientoAccesoCommand(
    string CodigoCredencial,
    Guid PuntoAccesoId,
    TipoMovimiento TipoMovimiento) : ICommand<RegistroAccesoDto>;

public sealed class RegistrarMovimientoAccesoCommandHandler : ICommandHandler<RegistrarMovimientoAccesoCommand, RegistroAccesoDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IPuntoAccesoRepository _puntoAccesoRepository;
    private readonly IRegistroAccesoRepository _registroAccesoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarMovimientoAccesoCommandHandler(
        ICredencialRepository credencialRepository,
        IPuntoAccesoRepository puntoAccesoRepository,
        IRegistroAccesoRepository registroAccesoRepository,
        IUnitOfWork unitOfWork)
    {
        _credencialRepository = credencialRepository;
        _puntoAccesoRepository = puntoAccesoRepository;
        _registroAccesoRepository = registroAccesoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegistroAccesoDto> HandleAsync(RegistrarMovimientoAccesoCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.CodigoCredencial))
            throw new DomainException("El código de la credencial es obligatorio.");

        var puntoAcceso = await _puntoAccesoRepository.ObtenerPorIdAsync(command.PuntoAccesoId, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el punto de acceso con ID '{command.PuntoAccesoId}'.");

        var codigoNormalizado = command.CodigoCredencial.Trim().ToUpperInvariant();
        var credencial = await _credencialRepository.ObtenerPorCodigoAsync(codigoNormalizado, cancellationToken);

        var registro = RegistroAcceso.EvaluarYRegistrar(
            codigoNormalizado,
            puntoAcceso,
            command.TipoMovimiento,
            credencial);

        await _registroAccesoRepository.AgregarAsync(registro, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RegistroAccesoDto.FromEntity(registro, credencial?.Empleado, puntoAcceso);
    }
}
