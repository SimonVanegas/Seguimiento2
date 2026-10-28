using Microsoft.EntityFrameworkCore;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Infrastructure.Persistence.Repositories;

public class CredencialRepository : ICredencialRepository
{
    private readonly SgcaeDbContext _dbContext;

    public CredencialRepository(SgcaeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Credencial?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codigo) || codigo.Trim().Length < 3 || codigo.Trim().Length > 50)
            return null;

        var codigoVo = CodigoCredencial.Crear(codigo);
        return await _dbContext.Credenciales
            .Include(c => c.Empleado)
            .FirstOrDefaultAsync(c => c.Codigo == codigoVo, cancellationToken);
    }

    public async Task<IReadOnlyList<Credencial>> ObtenerPorEmpleadoIdAsync(Guid empleadoId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Credenciales
            .AsNoTracking()
            .Include(c => c.Empleado)
            .Where(c => c.EmpleadoId == empleadoId)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(Credencial credencial, CancellationToken cancellationToken = default)
    {
        await _dbContext.Credenciales.AddAsync(credencial, cancellationToken);
    }

    public void Actualizar(Credencial credencial)
    {
        _dbContext.Credenciales.Update(credencial);
    }
}
