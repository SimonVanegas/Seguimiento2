using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Domain.Repositories;

namespace SGCAE.Application.Features.Empleados.Queries;

/// <summary>
/// Consulta con la que listo todos los empleados registrados (RF-01).
/// </summary>
public sealed record ListarEmpleadosQuery : IQuery<IReadOnlyList<EmpleadoDto>>;

public sealed class ListarEmpleadosQueryHandler : IQueryHandler<ListarEmpleadosQuery, IReadOnlyList<EmpleadoDto>>
{
    private readonly IEmpleadoRepository _empleadoRepository;

    public ListarEmpleadosQueryHandler(IEmpleadoRepository empleadoRepository)
    {
        _empleadoRepository = empleadoRepository;
    }

    public async Task<IReadOnlyList<EmpleadoDto>> HandleAsync(ListarEmpleadosQuery query, CancellationToken cancellationToken = default)
    {
        var empleados = await _empleadoRepository.ListarTodosAsync(cancellationToken);
        return empleados.Select(EmpleadoDto.FromEntity).ToList();
    }
}
