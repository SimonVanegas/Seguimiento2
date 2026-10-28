namespace SGCAE.Domain.Repositories;

/// <summary>
/// Defino el contrato de unidad de trabajo para confirmar mis transacciones en la base de datos.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
