namespace SGCAE.Domain.Enums;

/// <summary>
/// Defino los estados posibles para controlar si una credencial puede utilizarse (RF-02, RF-03).
/// </summary>
public enum EstadoCredencial
{
    Vigente = 1,
    Bloqueada = 2,
    Revocada = 3
}
