using SGCAE.Domain.Exceptions;

namespace SGCAE.Domain.ValueObjects;

/// <summary>
/// Implemento este objeto de valor para validar y asegurar la identificación única de un empleado (RF-01).
/// </summary>
public sealed record DocumentoIdentidad
{
    public string Valor { get; }

    private DocumentoIdentidad(string valor)
    {
        Valor = valor;
    }

    public static DocumentoIdentidad Crear(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("La identificación del empleado es obligatoria.");

        var normalizado = valor.Trim();
        if (normalizado.Length < 4 || normalizado.Length > 30)
            throw new DomainException("La identificación del empleado debe tener entre 4 y 30 caracteres.");

        return new DocumentoIdentidad(normalizado);
    }

    public override string ToString() => Valor;
}
