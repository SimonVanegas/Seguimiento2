using Microsoft.EntityFrameworkCore;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Enums;
using SGCAE.Domain.Repositories;

namespace SGCAE.Infrastructure.Persistence.Repositories;

public class RegistroAccesoRepository : IRegistroAccesoRepository
{
    private readonly SgcaeDbContext _dbContext;

    public RegistroAccesoRepository(SgcaeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AgregarAsync(RegistroAcceso registroAcceso, CancellationToken cancellationToken = default)
    {
        await _dbContext.RegistrosAcceso.AddAsync(registroAcceso, cancellationToken);
    }

    public async Task<(IReadOnlyList<RegistroAcceso> Items, int TotalCount)> ConsultarHistorialPaginadoAsync(
        Guid? empleadoId,
        DateTime? fechaInicio,
        DateTime? fechaFin,
        Guid? puntoAccesoId,
        ResultadoAcceso? resultado,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.RegistrosAcceso
            .AsNoTracking()
            .Include(r => r.Empleado)
            .Include(r => r.PuntoAcceso)
            .AsQueryable();

        if (empleadoId.HasValue)
            query = query.Where(r => r.EmpleadoId == empleadoId.Value);

        if (fechaInicio.HasValue)
            query = query.Where(r => r.FechaHora >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(r => r.FechaHora <= fechaFin.Value);

        if (puntoAccesoId.HasValue)
            query = query.Where(r => r.PuntoAccesoId == puntoAccesoId.Value);

        if (resultado.HasValue)
            query = query.Where(r => r.Resultado == resultado.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.FechaHora)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
