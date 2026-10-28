using SGCAE.Domain.Enums;
using SGCAE.Domain.Exceptions;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Domain.Entities;

/// <summary>
/// Modelo la entidad de dominio Empleado con sus invariantes y reglas de negocio (RF-01).
/// </summary>
public class Empleado
{
    public Guid Id { get; private set; }
    public DocumentoIdentidad Identificacion { get; private set; } = null!;
    public string Nombres { get; private set; } = null!;
    public string Apellidos { get; private set; } = null!;
    public CorreoElectronico Correo { get; private set; } = null!;
    public string Cargo { get; private set; } = null!;
    public string AreaOrganizacional { get; private set; } = null!;
    public EstadoLaboral EstadoLaboral { get; private set; }
    public DateTime FechaRegistro { get; private set; }

    private Empleado()
    {
    }

    public static Empleado Registrar(
        DocumentoIdentidad identificacion,
        string nombres,
        string apellidos,
        CorreoElectronico correo,
        string cargo,
        string areaOrganizacional)
    {
        ValidarCamposTexto(nombres, apellidos, cargo, areaOrganizacional);

        return new Empleado
        {
            Id = Guid.NewGuid(),
            Identificacion = identificacion ?? throw new DomainException("La identificación es obligatoria."),
            Nombres = nombres.Trim(),
            Apellidos = apellidos.Trim(),
            Correo = correo ?? throw new DomainException("El correo electrónico es obligatorio."),
            Cargo = cargo.Trim(),
            AreaOrganizacional = areaOrganizacional.Trim(),
            EstadoLaboral = EstadoLaboral.Activo,
            FechaRegistro = DateTime.UtcNow
        };
    }

    public void ActualizarDatos(
        string nombres,
        string apellidos,
        CorreoElectronico correo,
        string cargo,
        string areaOrganizacional)
    {
        ValidarCamposTexto(nombres, apellidos, cargo, areaOrganizacional);

        Nombres = nombres.Trim();
        Apellidos = apellidos.Trim();
        Correo = correo ?? throw new DomainException("El correo electrónico es obligatorio.");
        Cargo = cargo.Trim();
        AreaOrganizacional = areaOrganizacional.Trim();
    }

    public void Desactivar()
    {
        if (EstadoLaboral == EstadoLaboral.Inactivo)
            throw new DomainException("El empleado ya se encuentra en estado laboral inactivo.");

        EstadoLaboral = EstadoLaboral.Inactivo;
    }

    public bool EstaActivo() => EstadoLaboral == EstadoLaboral.Activo;

    private static void ValidarCamposTexto(string nombres, string apellidos, string cargo, string areaOrganizacional)
    {
        if (string.IsNullOrWhiteSpace(nombres))
            throw new DomainException("Los nombres del empleado son obligatorios.");
        if (string.IsNullOrWhiteSpace(apellidos))
            throw new DomainException("Los apellidos del empleado son obligatorios.");
        if (string.IsNullOrWhiteSpace(cargo))
            throw new DomainException("El cargo del empleado es obligatorio.");
        if (string.IsNullOrWhiteSpace(areaOrganizacional))
            throw new DomainException("El área organizacional del empleado es obligatoria.");
    }
}
