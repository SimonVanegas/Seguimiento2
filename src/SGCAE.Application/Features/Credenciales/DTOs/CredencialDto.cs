using SGCAE.Domain.Entities;

namespace SGCAE.Application.Features.Credenciales.DTOs;

public sealed record CredencialDto(
    Guid Id,
    string Codigo,
    Guid EmpleadoId,
    string? NombreEmpleado,
    string Estado,
    bool PuedeUtilizarse,
    DateTime FechaAsignacion)
{
    public static CredencialDto FromEntity(Credencial credencial) => new(
        credencial.Id,
        credencial.Codigo.Valor,
        credencial.EmpleadoId,
        credencial.Empleado is not null ? $"{credencial.Empleado.Nombres} {credencial.Empleado.Apellidos}" : null,
        credencial.Estado.ToString(),
        credencial.PuedeUtilizarse(),
        credencial.FechaAsignacion);
}
