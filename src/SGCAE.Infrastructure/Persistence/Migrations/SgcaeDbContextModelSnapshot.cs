using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SGCAE.Infrastructure.Persistence.Migrations;

[DbContext(typeof(SgcaeDbContext))]
partial class SgcaeDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.5")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("SGCAE.Domain.Entities.Empleado", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Apellidos")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.Property<string>("AreaOrganizacional")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.Property<string>("Cargo")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.Property<string>("Correo")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)")
                .HasColumnName("CorreoElectronico");

            b.Property<string>("EstadoLaboral")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<DateTime>("FechaRegistro")
                .HasColumnType("datetime2");

            b.Property<string>("Identificacion")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("nvarchar(30)")
                .HasColumnName("Identificacion");

            b.Property<string>("Nombres")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.HasKey("Id");

            b.HasIndex("Identificacion")
                .IsUnique()
                .HasDatabaseName("IX_Empleados_Identificacion");

            b.ToTable("Empleados", (string)null);
        });

        modelBuilder.Entity("SGCAE.Domain.Entities.PuntoAcceso", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Descripcion")
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("nvarchar(250)");

            b.Property<bool>("EsAreaRestringida")
                .HasColumnType("bit");

            b.Property<bool>("Habilitado")
                .HasColumnType("bit");

            b.Property<string>("Nombre")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.HasKey("Id");

            b.ToTable("PuntosAcceso", (string)null);
        });

        modelBuilder.Entity("SGCAE.Domain.Entities.Credencial", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Codigo")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)")
                .HasColumnName("Codigo");

            b.Property<Guid>("EmpleadoId")
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Estado")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<DateTime>("FechaAsignacion")
                .HasColumnType("datetime2");

            b.HasKey("Id");

            b.HasIndex("Codigo")
                .IsUnique()
                .HasDatabaseName("IX_Credenciales_Codigo");

            b.HasIndex("EmpleadoId");

            b.ToTable("Credenciales", (string)null);
        });

        modelBuilder.Entity("SGCAE.Domain.Entities.RegistroAcceso", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("CodigoCredencialUsado")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<Guid?>("CredencialId")
                .HasColumnType("uniqueidentifier");

            b.Property<Guid?>("EmpleadoId")
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime>("FechaHora")
                .HasColumnType("datetime2");

            b.Property<string>("Motivo")
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnType("nvarchar(300)");

            b.Property<Guid>("PuntoAccesoId")
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Resultado")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<string>("TipoMovimiento")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.HasKey("Id");

            b.HasIndex("CredencialId");
            b.HasIndex("EmpleadoId");
            b.HasIndex("FechaHora");
            b.HasIndex("PuntoAccesoId");
            b.HasIndex("Resultado");

            b.ToTable("RegistrosAcceso", (string)null);
        });

        modelBuilder.Entity("SGCAE.Domain.Entities.Credencial", b =>
        {
            b.HasOne("SGCAE.Domain.Entities.Empleado", "Empleado")
                .WithMany()
                .HasForeignKey("EmpleadoId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Empleado");
        });

        modelBuilder.Entity("SGCAE.Domain.Entities.RegistroAcceso", b =>
        {
            b.HasOne("SGCAE.Domain.Entities.Credencial", "Credencial")
                .WithMany()
                .HasForeignKey("CredencialId")
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne("SGCAE.Domain.Entities.Empleado", "Empleado")
                .WithMany()
                .HasForeignKey("EmpleadoId")
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne("SGCAE.Domain.Entities.PuntoAcceso", "PuntoAcceso")
                .WithMany()
                .HasForeignKey("PuntoAccesoId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Credencial");
            b.Navigation("Empleado");
            b.Navigation("PuntoAcceso");
        });
#pragma warning restore 612, 618
    }
}
