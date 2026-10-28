namespace SGCAE.Application.Abstractions;

/// <summary>
/// Defino esta interfaz para identificar los comandos que modifican el estado del sistema (CQRS).
/// </summary>
public interface ICommand<out TResponse>
{
}

/// <summary>
/// Defino el manejador encargado de ejecutar la lógica de un comando de escritura (CQRS).
/// </summary>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
