namespace SGCAE.Application.Abstractions;

/// <summary>
/// Defino esta interfaz para las consultas de solo lectura sin efectos secundarios (CQRS).
/// </summary>
public interface IQuery<out TResponse>
{
}

/// <summary>
/// Defino el manejador encargado de resolver una consulta de lectura (CQRS).
/// </summary>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
