using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SGCAE.Api.Middleware;
using SGCAE.Application;
using SGCAE.Infrastructure;
using SGCAE.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Registro mis capas siguiendo los principios de Clean Architecture
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// Aplico las migraciones de EF Core automáticamente si mi instancia de SQL Server está disponible
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SgcaeDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "No se pudieron aplicar las migraciones automáticamente al iniciar. Verifique la conexión a SQL Server.");
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapOpenApi();

// Expongo la interfaz interactiva Swagger UI consumiendo el esquema OpenAPI nativo
app.MapGet("/swagger", () => Results.Content("""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <title>SGCAE API - Documentación REST</title>
        <link rel="stylesheet" href="https://unpkg.com/swagger-ui-dist@5.18.2/swagger-ui.css" />
    </head>
    <body>
        <div id="swagger-ui"></div>
        <script src="https://unpkg.com/swagger-ui-dist@5.18.2/swagger-ui-bundle.js"></script>
        <script>
            window.onload = () => {
                window.ui = SwaggerUIBundle({
                    url: '/openapi/v1.json',
                    dom_id: '#swagger-ui',
                    deepLinking: true,
                    presets: [SwaggerUIBundle.presets.apis]
                });
            };
        </script>
    </body>
    </html>
    """, "text/html")).ExcludeFromDescription();

app.UseHttpsRedirection();

// Redirigir la raíz hacia Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
