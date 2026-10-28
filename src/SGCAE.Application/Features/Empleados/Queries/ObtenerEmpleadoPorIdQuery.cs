using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.Empleados.Queries;

/// <summary>
/// Consulta con la que obtengo un empleado por su identificador único (RF-01).
/// </summary>
public sealed record ObtenerEmpleadoPorIdQuery(Guid Id) : IQuery<EmpleadoDto>;

public sealed class ObtenerEmpleadoPorIdQueryHandler : IQueryHandler<ObtenerEmpleadoPorIdQuery, EmpleadoDto>
{
    private readonly IEmpleadoRepository _empleadoRepository;

    public ObtenerEmpleadoPorIdQueryHandler(IEmpleadoRepository empleadoRepository)
    {
        _empleadoRepository = empleadoRepository;
    }

    public async Task<EmpleadoDto> HandleAsync(ObtenerEmpleadoPorIdQuery query, CancellationToken cancellationToken = default)
    {
        var empleado = await _empleadoRepository.ObtenerPorIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el empleado con ID '{query.Id}'.");

        return EmpleadoDto.FromEntity(empleado);
    }
}
