using SGCAE.Domain.Entities;

namespace SGCAE.Application.Features.PuntosAcceso.DTOs;

public sealed record PuntoAccesoDto(
    Guid Id,
    string Nombre,
    string Descripcion,
    bool EsAreaRestringida,
    bool Habilitado)
{
    public static PuntoAccesoDto FromEntity(PuntoAcceso puntoAcceso) => new(
        puntoAcceso.Id,
        puntoAcceso.Nombre,
        puntoAcceso.Descripcion,
        puntoAcceso.EsAreaRestringida,
        puntoAcceso.Habilitado);
}
