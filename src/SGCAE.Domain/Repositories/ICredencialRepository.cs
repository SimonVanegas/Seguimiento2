using SGCAE.Domain.Entities;

namespace SGCAE.Domain.Repositories;

/// <summary>
/// Defino el contrato de persistencia para gestionar las credenciales de acceso (RF-02).
/// </summary>
public interface ICredencialRepository
{
    Task<Credencial?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Credencial>> ObtenerPorEmpleadoIdAsync(Guid empleadoId, CancellationToken cancellationToken = default);
    Task AgregarAsync(Credencial credencial, CancellationToken cancellationToken = default);
    void Actualizar(Credencial credencial);
}
