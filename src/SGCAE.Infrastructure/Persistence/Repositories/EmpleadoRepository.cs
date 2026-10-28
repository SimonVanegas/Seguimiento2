using Microsoft.EntityFrameworkCore;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Repositories;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Infrastructure.Persistence.Repositories;

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly SgcaeDbContext _dbContext;

    public EmpleadoRepository(SgcaeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Empleado?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Empleados
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Empleado?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default)
    {
        var doc = DocumentoIdentidad.Crear(identificacion);
        return await _dbContext.Empleados
            .FirstOrDefaultAsync(e => e.Identificacion == doc, cancellationToken);
    }

    public async Task<IReadOnlyList<Empleado>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Empleados
            .AsNoTracking()
            .OrderBy(e => e.Apellidos)
            .ThenBy(e => e.Nombres)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(Empleado empleado, CancellationToken cancellationToken = default)
    {
        await _dbContext.Empleados.AddAsync(empleado, cancellationToken);
    }

    public void Actualizar(Empleado empleado)
    {
        _dbContext.Empleados.Update(empleado);
    }
}
