using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCAE.Domain.Entities;
using SGCAE.Domain.ValueObjects;

namespace SGCAE.Infrastructure.Persistence.Configurations;

public class CredencialConfiguration : IEntityTypeConfiguration<Credencial>
{
    public void Configure(EntityTypeBuilder<Credencial> builder)
    {
        builder.ToTable("Credenciales");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Codigo)
            .HasConversion(
                v => v.Valor,
                v => CodigoCredencial.Crear(v))
            .HasColumnName("Codigo")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Codigo)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_Codigo");

        builder.Property(c => c.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.FechaAsignacion)
            .IsRequired();

        builder.HasOne(c => c.Empleado)
            .WithMany()
            .HasForeignKey(c => c.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
