using Microsoft.Extensions.DependencyInjection;
using SGCAE.Application.Abstractions;
using SGCAE.Application.Features.Accesos.Commands;
using SGCAE.Application.Features.Accesos.DTOs;
using SGCAE.Application.Features.Accesos.Queries;
using SGCAE.Application.Features.Credenciales.Commands;
using SGCAE.Application.Features.Credenciales.DTOs;
using SGCAE.Application.Features.Credenciales.Queries;
using SGCAE.Application.Features.Empleados.Commands;
using SGCAE.Application.Features.Empleados.DTOs;
using SGCAE.Application.Features.Empleados.Queries;
using SGCAE.Application.Features.PuntosAcceso.Commands;
using SGCAE.Application.Features.PuntosAcceso.DTOs;
using SGCAE.Application.Features.PuntosAcceso.Queries;

namespace SGCAE.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registro mis casos de uso para RF-01: Empleados (Commands y Queries)
        services.AddScoped<ICommandHandler<RegistrarEmpleadoCommand, EmpleadoDto>, RegistrarEmpleadoCommandHandler>();
        services.AddScoped<ICommandHandler<ActualizarEmpleadoCommand, EmpleadoDto>, ActualizarEmpleadoCommandHandler>();
        services.AddScoped<ICommandHandler<DesactivarEmpleadoCommand, EmpleadoDto>, DesactivarEmpleadoCommandHandler>();
        services.AddScoped<IQueryHandler<ObtenerEmpleadoPorIdQuery, EmpleadoDto>, ObtenerEmpleadoPorIdQueryHandler>();
        services.AddScoped<IQueryHandler<ListarEmpleadosQuery, IReadOnlyList<EmpleadoDto>>, ListarEmpleadosQueryHandler>();

        // Registro mis casos de uso para RF-02: Credenciales (Commands y Queries)
        services.AddScoped<ICommandHandler<AsignarCredencialCommand, CredencialDto>, AsignarCredencialCommandHandler>();
        services.AddScoped<ICommandHandler<BloquearCredencialCommand, CredencialDto>, BloquearCredencialCommandHandler>();
        services.AddScoped<ICommandHandler<RevocarCredencialCommand, CredencialDto>, RevocarCredencialCommandHandler>();
        services.AddScoped<IQueryHandler<ConsultarEstadoCredencialQuery, CredencialDto>, ConsultarEstadoCredencialQueryHandler>();

        // Registro mis casos de uso para RF-03 y RF-04: Accesos e Historial Paginado (Commands y Queries)
        services.AddScoped<ICommandHandler<RegistrarMovimientoAccesoCommand, RegistroAccesoDto>, RegistrarMovimientoAccesoCommandHandler>();
        services.AddScoped<IQueryHandler<ConsultarHistorialAccesosQuery, PagedResult<RegistroAccesoDto>>, ConsultarHistorialAccesosQueryHandler>();

        // Registro mis casos de uso para RF-06: Puntos de Acceso (Commands y Queries)
        services.AddScoped<ICommandHandler<RegistrarPuntoAccesoCommand, PuntoAccesoDto>, RegistrarPuntoAccesoCommandHandler>();
        services.AddScoped<IQueryHandler<ListarPuntosAccesoQuery, IReadOnlyList<PuntoAccesoDto>>, ListarPuntosAccesoQueryHandler>();

        return services;
    }
}
