using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCAE.Domain.Entities;

namespace SGCAE.Infrastructure.Persistence.Configurations;

public class PuntoAccesoConfiguration : IEntityTypeConfiguration<PuntoAcceso>
{
    public void Configure(EntityTypeBuilder<PuntoAcceso> builder)
    {
        builder.ToTable("PuntosAcceso");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Descripcion)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(p => p.EsAreaRestringida)
            .IsRequired();

        builder.Property(p => p.Habilitado)
            .IsRequired();
    }
}
