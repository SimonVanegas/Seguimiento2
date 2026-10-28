using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Credenciales.DTOs;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Application.Features.Credenciales.Commands;

/// <summary>
/// Comando con el que bloqueo una credencial de acceso vigente (RF-02).
/// </summary>
public sealed record BloquearCredencialCommand(string Codigo) : ICommand<CredencialDto>;

public sealed class BloquearCredencialCommandHandler : ICommandHandler<BloquearCredencialCommand, CredencialDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BloquearCredencialCommandHandler(ICredencialRepository credencialRepository, IUnitOfWork unitOfWork)
    {
        _credencialRepository = credencialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CredencialDto> HandleAsync(BloquearCredencialCommand command, CancellationToken cancellationToken = default)
    {
        var codigo = CodigoCredencial.Crear(command.Codigo);
        var credencial = await _credencialRepository.ObtenerPorCodigoAsync(codigo.Valor, cancellationToken)
            ?? throw new NotFoundException($"No se encontró la credencial con código '{codigo.Valor}'.");

        credencial.Bloquear();

        _credencialRepository.Actualizar(credencial);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CredencialDto.FromEntity(credencial);
    }
}
