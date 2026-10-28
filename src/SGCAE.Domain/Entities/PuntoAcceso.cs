using SGCAE.Domain.Exceptions;

namespace SGCAE.Domain.Entities;

/// <summary>
/// Modelo la entidad de dominio PuntoAcceso para representar las áreas físicas de la organización (RF-06).
/// </summary>
public class PuntoAcceso
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public string Descripcion { get; private set; } = null!;
    public bool EsAreaRestringida { get; private set; }
    public bool Habilitado { get; private set; }

    private PuntoAcceso()
    {
    }

    public static PuntoAcceso Registrar(string nombre, string descripcion, bool esAreaRestringida, bool habilitado = true)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre del punto de acceso es obligatorio.");

        return new PuntoAcceso
        {
            Id = Guid.NewGuid(),
            Nombre = nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? string.Empty : descripcion.Trim(),
            EsAreaRestringida = esAreaRestringida,
            Habilitado = habilitado
        };
    }
}
