using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Credenciales.DTOs;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Application.Features.Credenciales.Commands;

/// <summary>
/// Comando con el que revoco definitivamente una credencial de acceso (RF-02).
/// </summary>
public sealed record RevocarCredencialCommand(string Codigo) : ICommand<CredencialDto>;

public sealed class RevocarCredencialCommandHandler : ICommandHandler<RevocarCredencialCommand, CredencialDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevocarCredencialCommandHandler(ICredencialRepository credencialRepository, IUnitOfWork unitOfWork)
    {
        _credencialRepository = credencialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CredencialDto> HandleAsync(RevocarCredencialCommand command, CancellationToken cancellationToken = default)
    {
        var codigo = CodigoCredencial.Crear(command.Codigo);
        var credencial = await _credencialRepository.ObtenerPorCodigoAsync(codigo.Valor, cancellationToken)
            ?? throw new NotFoundException($"No se encontró la credencial con código '{codigo.Valor}'.");

        credencial.Revocar();

        _credencialRepository.Actualizar(credencial);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CredencialDto.FromEntity(credencial);
    }
}
