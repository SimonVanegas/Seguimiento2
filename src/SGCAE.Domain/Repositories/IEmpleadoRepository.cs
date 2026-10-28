using SGCAE.Domain.Entities;

namespace SGCAE.Domain.Repositories;

/// <summary>
/// Defino el contrato de persistencia para gestionar los empleados (RF-01).
/// </summary>
public interface IEmpleadoRepository
{
    Task<Empleado?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Empleado?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Empleado>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(Empleado empleado, CancellationToken cancellationToken = default);
    void Actualizar(Empleado empleado);
}
