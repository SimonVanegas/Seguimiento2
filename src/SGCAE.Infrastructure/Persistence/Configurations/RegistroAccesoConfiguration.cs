using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCAE.Domain.Entities;

namespace SGCAE.Infrastructure.Persistence.Configurations;

public class RegistroAccesoConfiguration : IEntityTypeConfiguration<RegistroAcceso>
{
    public void Configure(EntityTypeBuilder<RegistroAcceso> builder)
    {
        builder.ToTable("RegistrosAcceso");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CodigoCredencialUsado)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.TipoMovimiento)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.FechaHora)
            .IsRequired();

        builder.Property(r => r.Resultado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.Motivo)
            .HasMaxLength(300)
            .IsRequired();

        builder.HasOne(r => r.Credencial)
            .WithMany()
            .HasForeignKey(r => r.CredencialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Empleado)
            .WithMany()
            .HasForeignKey(r => r.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.PuntoAcceso)
            .WithMany()
            .HasForeignKey(r => r.PuntoAccesoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.EmpleadoId);
        builder.HasIndex(r => r.PuntoAccesoId);
        builder.HasIndex(r => r.FechaHora);
        builder.HasIndex(r => r.Resultado);
    }
}
