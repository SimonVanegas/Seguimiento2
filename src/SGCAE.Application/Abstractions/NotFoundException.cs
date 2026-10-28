namespace SGCAE.Application.Abstractions;

/// <summary>
/// Lanzo esta excepción cuando busco un recurso en el sistema y no existe.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
