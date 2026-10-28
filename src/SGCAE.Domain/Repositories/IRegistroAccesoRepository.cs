using SGCAE.Domain.Entities;
using SGCAE.Domain.Enums;

namespace SGCAE.Domain.Repositories;

/// <summary>
/// Defino el contrato de persistencia para registrar accesos y consultar el historial paginado (RF-03, RF-04).
/// </summary>
public interface IRegistroAccesoRepository
{
    Task AgregarAsync(RegistroAcceso registroAcceso, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<RegistroAcceso> Items, int TotalCount)> ConsultarHistorialPaginadoAsync(
        Guid? empleadoId,
        DateTime? fechaInicio,
        DateTime? fechaFin,
        Guid? puntoAccesoId,
        ResultadoAcceso? resultado,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
