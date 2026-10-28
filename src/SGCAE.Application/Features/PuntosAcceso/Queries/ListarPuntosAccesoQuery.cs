using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.PuntosAcceso.DTOs;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.PuntosAcceso.Queries;

/// <summary>
/// Consulta con la que listo los puntos de acceso físicos e identifico cuáles están habilitados (RF-06).
/// </summary>
public sealed record ListarPuntosAccesoQuery(bool? SoloHabilitados = null) : IQuery<IReadOnlyList<PuntoAccesoDto>>;

public sealed class ListarPuntosAccesoQueryHandler : IQueryHandler<ListarPuntosAccesoQuery, IReadOnlyList<PuntoAccesoDto>>
{
    private readonly IPuntoAccesoRepository _puntoAccesoRepository;

    public ListarPuntosAccesoQueryHandler(IPuntoAccesoRepository puntoAccesoRepository)
    {
        _puntoAccesoRepository = puntoAccesoRepository;
    }

    public async Task<IReadOnlyList<PuntoAccesoDto>> HandleAsync(ListarPuntosAccesoQuery query, CancellationToken cancellationToken = default)
    {
        var puntos = await _puntoAccesoRepository.ListarAsync(query.SoloHabilitados, cancellationToken);
        return puntos.Select(PuntoAccesoDto.FromEntity).ToList();
    }
}
