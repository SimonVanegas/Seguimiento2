using SGCAE.Domain.Enums;
using SGCAE.Domain.Exceptions;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Domain.Entities;

/// <summary>
/// Modelo la entidad de dominio Credencial para controlar la asignación, bloqueo y revocación de accesos (RF-02).
/// </summary>
public class Credencial
{
    public Guid Id { get; private set; }
    public CodigoCredencial Codigo { get; private set; } = null!;
    public Guid EmpleadoId { get; private set; }
    public Empleado? Empleado { get; private set; }
    public EstadoCredencial Estado { get; private set; }
    public DateTime FechaAsignacion { get; private set; }

    private Credencial()
    {
    }

    public static Credencial Asignar(CodigoCredencial codigo, Empleado empleado)
    {
        if (empleado is null)
            throw new DomainException("Se requiere un empleado válido para asignar una credencial.");

        if (!empleado.EstaActivo())
            throw new DomainException("No se puede asignar una credencial a un empleado con vínculo laboral inactivo.");

        return new Credencial
        {
            Id = Guid.NewGuid(),
            Codigo = codigo ?? throw new DomainException("El código de la credencial es obligatorio."),
            EmpleadoId = empleado.Id,
            Empleado = empleado,
            Estado = EstadoCredencial.Vigente,
            FechaAsignacion = DateTime.UtcNow
        };
    }

    public void Bloquear()
    {
        if (Estado == EstadoCredencial.Revocada)
            throw new DomainException("No se puede bloquear una credencial que ya ha sido revocada.");

        if (Estado == EstadoCredencial.Bloqueada)
            throw new DomainException("La credencial ya se encuentra bloqueada.");

        Estado = EstadoCredencial.Bloqueada;
    }

    public void Revocar()
    {
        if (Estado == EstadoCredencial.Revocada)
            throw new DomainException("La credencial ya se encuentra revocada.");

        Estado = EstadoCredencial.Revocada;
    }

    public bool PuedeUtilizarse() => Estado == EstadoCredencial.Vigente;
}
