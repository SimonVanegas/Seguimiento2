using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Accesos.DTOs;
using SGCAE.Domain.Enums;
using SGCAE.Domain.Exceptions;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.Accesos.Queries;

/// <summary>
/// Consulta paginada con la que obtengo el historial de accesos filtrando por empleado, fechas, punto de acceso y resultado (RF-04).
/// </summary>
public sealed record ConsultarHistorialAccesosQuery(
    Guid? EmpleadoId = null,
    DateTime? FechaInicio = null,
    DateTime? FechaFin = null,
    Guid? PuntoAccesoId = null,
    ResultadoAcceso? Resultado = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<RegistroAccesoDto>>;

public sealed class ConsultarHistorialAccesosQueryHandler
    : IQueryHandler<ConsultarHistorialAccesosQuery, PagedResult<RegistroAccesoDto>>
{
    private readonly IRegistroAccesoRepository _registroAccesoRepository;

    public ConsultarHistorialAccesosQueryHandler(IRegistroAccesoRepository registroAccesoRepository)
    {
        _registroAccesoRepository = registroAccesoRepository;
    }

    public async Task<PagedResult<RegistroAccesoDto>> HandleAsync(
        ConsultarHistorialAccesosQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.PageNumber < 1)
            throw new DomainException("El número de página debe ser mayor o igual a 1.");

        if (query.PageSize < 1 || query.PageSize > 100)
            throw new DomainException("El tamaño de página debe estar entre 1 y 100 registros.");

        if (query.FechaInicio.HasValue && query.FechaFin.HasValue && query.FechaInicio.Value > query.FechaFin.Value)
            throw new DomainException("La fecha de inicio no puede ser mayor que la fecha de fin.");

        var (items, totalCount) = await _registroAccesoRepository.ConsultarHistorialPaginadoAsync(
            query.EmpleadoId,
            query.FechaInicio,
            query.FechaFin,
            query.PuntoAccesoId,
            query.Resultado,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var dtos = items.Select(r => RegistroAccesoDto.FromEntity(r)).ToList();

        return new PagedResult<RegistroAccesoDto>(
            dtos,
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}
