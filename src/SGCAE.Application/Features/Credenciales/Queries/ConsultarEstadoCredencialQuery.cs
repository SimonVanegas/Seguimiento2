using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Credenciales.DTOs;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Application.Features.Credenciales.Queries;

/// <summary>
/// Consulta con la que verifico el estado de una credencial y determino si puede utilizarse (RF-02).
/// </summary>
public sealed record ConsultarEstadoCredencialQuery(string Codigo) : IQuery<CredencialDto>;

public sealed class ConsultarEstadoCredencialQueryHandler : IQueryHandler<ConsultarEstadoCredencialQuery, CredencialDto>
{
    private readonly ICredencialRepository _credencialRepository;

    public ConsultarEstadoCredencialQueryHandler(ICredencialRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task<CredencialDto> HandleAsync(ConsultarEstadoCredencialQuery query, CancellationToken cancellationToken = default)
    {
        var codigo = CodigoCredencial.Crear(query.Codigo);
        var credencial = await _credencialRepository.ObtenerPorCodigoAsync(codigo.Valor, cancellationToken)
            ?? throw new NotFoundException($"No se encontró la credencial con código '{codigo.Valor}'.");

        return CredencialDto.FromEntity(credencial);
    }
}
