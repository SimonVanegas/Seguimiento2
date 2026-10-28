using SGCAE.Domain.Enums;
using SGCAE.Domain.Exceptions;

namespace SGCAE.Domain.Entities;

/// <summary>
/// Modelo la entidad RegistroAcceso donde evalúo y guardo tanto los accesos autorizados como los rechazados (RF-03, RF-04).
/// </summary>
public class RegistroAcceso
{
    public Guid Id { get; private set; }
    public string CodigoCredencialUsado { get; private set; } = null!;
    public Guid? CredencialId { get; private set; }
    public Credencial? Credencial { get; private set; }
    public Guid? EmpleadoId { get; private set; }
    public Empleado? Empleado { get; private set; }
    public Guid PuntoAccesoId { get; private set; }
    public PuntoAcceso? PuntoAcceso { get; private set; }
    public TipoMovimiento TipoMovimiento { get; private set; }
    public DateTime FechaHora { get; private set; }
    public ResultadoAcceso Resultado { get; private set; }
    public string Motivo { get; private set; } = null!;

    private RegistroAcceso()
    {
    }

    public static RegistroAcceso EvaluarYRegistrar(
        string codigoCredencial,
        PuntoAcceso puntoAcceso,
        TipoMovimiento tipoMovimiento,
        Credencial? credencial)
    {
        if (string.IsNullOrWhiteSpace(codigoCredencial))
            throw new DomainException("El código de la credencial es obligatorio para registrar un movimiento de acceso.");

        if (puntoAcceso is null)
            throw new DomainException("El punto de acceso especificado no es válido.");

        if (!Enum.IsDefined(typeof(TipoMovimiento), tipoMovimiento))
            throw new DomainException("El tipo de movimiento debe ser Entrada o Salida.");

        var codigoNormalizado = codigoCredencial.Trim().ToUpperInvariant();
        var empleado = credencial?.Empleado;

        if (!puntoAcceso.Habilitado)
        {
            return CrearRegistro(
                codigoNormalizado,
                credencial?.Id,
                empleado?.Id,
                puntoAcceso.Id,
                tipoMovimiento,
                ResultadoAcceso.Rechazado,
                "Acceso rechazado: el punto de acceso no se encuentra habilitado.");
        }

        if (credencial is null)
        {
            return CrearRegistro(
                codigoNormalizado,
                null,
                null,
                puntoAcceso.Id,
                tipoMovimiento,
                ResultadoAcceso.Rechazado,
                "Acceso rechazado: la credencial no existe en el sistema.");
        }

        if (!credencial.PuedeUtilizarse())
        {
            return CrearRegistro(
                codigoNormalizado,
                credencial.Id,
                empleado?.Id,
                puntoAcceso.Id,
                tipoMovimiento,
                ResultadoAcceso.Rechazado,
                $"Acceso rechazado: la credencial se encuentra en estado {credencial.Estado}.");
        }

        if (empleado is null || !empleado.EstaActivo())
        {
            return CrearRegistro(
                codigoNormalizado,
                credencial.Id,
                empleado?.Id,
                puntoAcceso.Id,
                tipoMovimiento,
                ResultadoAcceso.Rechazado,
                "Acceso rechazado: el vínculo laboral del empleado se encuentra inactivo.");
        }

        return CrearRegistro(
            codigoNormalizado,
            credencial.Id,
            empleado.Id,
            puntoAcceso.Id,
            tipoMovimiento,
            ResultadoAcceso.Autorizado,
            "Acceso autorizado.");
    }

    private static RegistroAcceso CrearRegistro(
        string codigoCredencial,
        Guid? credencialId,
        Guid? empleadoId,
        Guid puntoAccesoId,
        TipoMovimiento tipoMovimiento,
        ResultadoAcceso resultado,
        string motivo)
    {
        return new RegistroAcceso
        {
            Id = Guid.NewGuid(),
            CodigoCredencialUsado = codigoCredencial,
            CredencialId = credencialId,
            EmpleadoId = empleadoId,
            PuntoAccesoId = puntoAccesoId,
            TipoMovimiento = tipoMovimiento,
            FechaHora = DateTime.UtcNow,
            Resultado = resultado,
            Motivo = motivo
        };
    }
}
