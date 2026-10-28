using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCAE.Domain.Entities;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Infrastructure.Persistence.Configurations;

public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {
        builder.ToTable("Empleados");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Identificacion)
            .HasConversion(
                v => v.Valor,
                v => DocumentoIdentidad.Crear(v))
            .HasColumnName("Identificacion")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(e => e.Identificacion)
            .IsUnique()
            .HasDatabaseName("IX_Empleados_Identificacion");

        builder.Property(e => e.Nombres)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Apellidos)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Correo)
            .HasConversion(
                v => v.Valor,
                v => CorreoElectronico.Crear(v))
            .HasColumnName("CorreoElectronico")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Cargo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.AreaOrganizacional)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.EstadoLaboral)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.FechaRegistro)
            .IsRequired();
    }
}
