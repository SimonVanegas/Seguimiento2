using SGCAE.Domain.Entities;

namespace SGCAE.Application.Features.Accesos.DTOs;

public sealed record RegistroAccesoDto(
    Guid Id,
    string CodigoCredencialUsado,
    Guid? EmpleadoId,
    string? NombreEmpleado,
    Guid PuntoAccesoId,
    string? NombrePuntoAcceso,
    string TipoMovimiento,
    DateTime FechaHora,
    string Resultado,
    string Motivo)
{
    public static RegistroAccesoDto FromEntity(RegistroAcceso registro, Empleado? empleado = null, PuntoAcceso? puntoAcceso = null)
    {
        var emp = empleado ?? registro.Empleado;
        var punto = puntoAcceso ?? registro.PuntoAcceso;

        return new RegistroAccesoDto(
            registro.Id,
            registro.CodigoCredencialUsado,
            registro.EmpleadoId,
            emp is not null ? $"{emp.Nombres} {emp.Apellidos}" : null,
            registro.PuntoAccesoId,
            punto?.Nombre,
            registro.TipoMovimiento.ToString(),
            registro.FechaHora,
            registro.Resultado.ToString(),
            registro.Motivo);
    }
}
