namespace SGCAE.Domain.Exceptions;

/// <summary>
/// Utilizo esta excepción para controlar cuando se incumple alguna regla o invariante del dominio.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
