using SGCAE.Domain.Entities;

namespace SGCAE.Domain.Repositories;

/// <summary>
/// Defino el contrato de persistencia para administrar los puntos de acceso (RF-06).
/// </summary>
public interface IPuntoAccesoRepository
{
    Task<PuntoAcceso?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PuntoAcceso>> ListarAsync(bool? soloHabilitados = null, CancellationToken cancellationToken = default);
    Task AgregarAsync(PuntoAcceso puntoAcceso, CancellationToken cancellationToken = default);
}
