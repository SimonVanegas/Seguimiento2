using System.Text.RegularExpressions;
using SGCAE.Domain.Exceptions;

namespace SGCAE.Domain.ValueObjects;

/// <summary>
/// Implemento este objeto de valor para garantizar que el correo electrónico del empleado tenga un formato válido (RF-01).
/// </summary>
public sealed record CorreoElectronico
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Valor { get; }

    private CorreoElectronico(string valor)
    {
        Valor = valor;
    }

    public static CorreoElectronico Crear(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("El correo electrónico del empleado es obligatorio.");

        var normalizado = valor.Trim().ToLowerInvariant();
        if (normalizado.Length > 150 || !EmailRegex.IsMatch(normalizado))
            throw new DomainException("El formato del correo electrónico no es válido.");

        return new CorreoElectronico(normalizado);
    }

    public override string ToString() => Valor;
}
