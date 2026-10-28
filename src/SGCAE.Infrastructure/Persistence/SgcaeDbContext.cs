using Microsoft.EntityFrameworkCore;
using SGCAE.Domain.Entities;
using SGCAE.Domain.Repositories;

namespace SGCAE.Infrastructure.Persistence;

/// <summary>
/// Configuro mi contexto de base de datos con Entity Framework Core sobre SQL Server.
/// </summary>
public class SgcaeDbContext : DbContext, IUnitOfWork
{
    public SgcaeDbContext(DbContextOptions<SgcaeDbContext> options) : base(options)
    {
    }

    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Credencial> Credenciales => Set<Credencial>();
    public DbSet<PuntoAcceso> PuntosAcceso => Set<PuntoAcceso>();
    public DbSet<RegistroAcceso> RegistrosAcceso => Set<RegistroAcceso>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SgcaeDbContext).Assembly);
    }
}
