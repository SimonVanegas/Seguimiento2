using SGCAE.Domain.Entities;

namespace SGCAE.Application.Features.Empleados.DTOs;

public sealed record EmpleadoDto(
    Guid Id,
    string Identificacion,
    string Nombres,
    string Apellidos,
    string CorreoElectronico,
    string Cargo,
    string AreaOrganizacional,
    string EstadoLaboral,
    DateTime FechaRegistro)
{
    public static EmpleadoDto FromEntity(Empleado empleado) => new(
        empleado.Id,
        empleado.Identificacion.Valor,
        empleado.Nombres,
        empleado.Apellidos,
        empleado.Correo.Valor,
        empleado.Cargo,
        empleado.AreaOrganizacional,
        empleado.EstadoLaboral.ToString(),
        empleado.FechaRegistro);
}
