using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SGCAE.Infrastructure.Persistence;

#nullable disable

namespace SGCAE.Infrastructure.Persistence.Migrations;

[DbContext(typeof(SgcaeDbContext))]
[Migration("20261028000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Empleados",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Identificacion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CorreoElectronico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                AreaOrganizacional = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                EstadoLaboral = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Empleados", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PuntosAcceso",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                EsAreaRestringida = table.Column<bool>(type: "bit", nullable: false),
                Habilitado = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PuntosAcceso", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Credenciales",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                EmpleadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Credenciales", x => x.Id);
                table.ForeignKey(
                    name: "FK_Credenciales_Empleados_EmpleadoId",
                    column: x => x.EmpleadoId,
                    principalTable: "Empleados",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "RegistrosAcceso",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CodigoCredencialUsado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CredencialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EmpleadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PuntoAccesoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TipoMovimiento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                Resultado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Motivo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RegistrosAcceso", x => x.Id);
                table.ForeignKey(
                    name: "FK_RegistrosAcceso_Credenciales_CredencialId",
                    column: x => x.CredencialId,
                    principalTable: "Credenciales",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_RegistrosAcceso_Empleados_EmpleadoId",
                    column: x => x.EmpleadoId,
                    principalTable: "Empleados",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_RegistrosAcceso_PuntosAcceso_PuntoAccesoId",
                    column: x => x.PuntoAccesoId,
                    principalTable: "PuntosAcceso",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Empleados_Identificacion",
            table: "Empleados",
            column: "Identificacion",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Credenciales_Codigo",
            table: "Credenciales",
            column: "Codigo",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Credenciales_EmpleadoId",
            table: "Credenciales",
            column: "EmpleadoId");

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosAcceso_CredencialId",
            table: "RegistrosAcceso",
            column: "CredencialId");

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosAcceso_EmpleadoId",
            table: "RegistrosAcceso",
            column: "EmpleadoId");

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosAcceso_FechaHora",
            table: "RegistrosAcceso",
            column: "FechaHora");

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosAcceso_PuntoAccesoId",
            table: "RegistrosAcceso",
            column: "PuntoAccesoId");

        migrationBuilder.CreateIndex(
            name: "IX_RegistrosAcceso_Resultado",
            table: "RegistrosAcceso",
            column: "Resultado");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "RegistrosAcceso");
        migrationBuilder.DropTable(name: "Credenciales");
        migrationBuilder.DropTable(name: "PuntosAcceso");
        migrationBuilder.DropTable(name: "Empleados");
    }
}
