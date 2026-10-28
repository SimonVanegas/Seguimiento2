using Microsoft.EntityFrameworkCore;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Repositories;

namespace SGCAE.Infrastructure.Persistence.Repositories;

public class PuntoAccesoRepository : IPuntoAccesoRepository
{
    private readonly SgcaeDbContext _dbContext;

    public PuntoAccesoRepository(SgcaeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PuntoAcceso?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PuntosAcceso
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<PuntoAcceso>> ListarAsync(bool? soloHabilitados = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.PuntosAcceso.AsNoTracking().AsQueryable();

        if (soloHabilitados.HasValue)
            query = query.Where(p => p.Habilitado == soloHabilitados.Value);

        return await query
            .OrderBy(p => p.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(PuntoAcceso puntoAcceso, CancellationToken cancellationToken = default)
    {
        await _dbContext.PuntosAcceso.AddAsync(puntoAcceso, cancellationToken);
    }
}
