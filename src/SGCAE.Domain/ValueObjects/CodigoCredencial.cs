using SGCAE.Domain.Exceptions;

namespace SGCAE.Domain.ValueObjects;

/// <summary>
/// Implemento este objeto de valor para normalizar y validar el código único de una credencial de acceso (RF-02, RF-03).
/// </summary>
public sealed record CodigoCredencial
{
    public string Valor { get; }

    private CodigoCredencial(string valor)
    {
        Valor = valor;
    }

    public static CodigoCredencial Crear(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("El código de la credencial es obligatorio.");

        var normalizado = valor.Trim().ToUpperInvariant();
        if (normalizado.Length < 3 || normalizado.Length > 50)
            throw new DomainException("El código de la credencial debe tener entre 3 y 50 caracteres.");

        return new CodigoCredencial(normalizado);
    }

    public override string ToString() => Valor;
}
