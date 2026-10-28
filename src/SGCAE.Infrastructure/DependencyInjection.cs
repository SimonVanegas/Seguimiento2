using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGCAE.Domain.Repositories;
using SGCAE.Infrastructure.Persistence;
using SGCAE.Infrastructure.Persistence.Repositories;

namespace SGCAE.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection' para SQL Server.");

        services.AddDbContext<SgcaeDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SgcaeDbContext>());
        services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
        services.AddScoped<ICredencialRepository, CredencialRepository>();
        services.AddScoped<IPuntoAccesoRepository, PuntoAccesoRepository>();
        services.AddScoped<IRegistroAccesoRepository, RegistroAccesoRepository>();

        return services;
    }
}
