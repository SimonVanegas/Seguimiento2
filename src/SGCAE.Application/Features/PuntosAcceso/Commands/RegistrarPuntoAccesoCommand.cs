using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.PuntosAcceso.DTOs;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.PuntosAcceso.Commands;

/// <summary>
/// Comando con el que registro un nuevo punto de acceso físico de la organización (RF-06).
/// </summary>
public sealed record RegistrarPuntoAccesoCommand(
    string Nombre,
    string Descripcion,
    bool EsAreaRestringida,
    bool Habilitado = true) : ICommand<PuntoAccesoDto>;

public sealed class RegistrarPuntoAccesoCommandHandler : ICommandHandler<RegistrarPuntoAccesoCommand, PuntoAccesoDto>
{
    private readonly IPuntoAccesoRepository _puntoAccesoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarPuntoAccesoCommandHandler(IPuntoAccesoRepository puntoAccesoRepository, IUnitOfWork unitOfWork)
    {
        _puntoAccesoRepository = puntoAccesoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PuntoAccesoDto> HandleAsync(RegistrarPuntoAccesoCommand command, CancellationToken cancellationToken = default)
    {
        var puntoAcceso = PuntoAcceso.Registrar(
            command.Nombre,
            command.Descripcion,
            command.EsAreaRestringida,
            command.Habilitado);

        await _puntoAccesoRepository.AgregarAsync(puntoAcceso, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PuntoAccesoDto.FromEntity(puntoAcceso);
    }
}
