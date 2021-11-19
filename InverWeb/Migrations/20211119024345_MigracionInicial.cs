using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InverWeb.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Celular = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    EmpresaNombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmpresaCargo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EmpresaSalario = table.Column<double>(type: "float", nullable: false),
                    EmpresaDireccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmpresaTelefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EmpresaFechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "ID", "Cedula", "Celular", "Correo", "Direccion", "EmpresaCargo", "EmpresaDireccion", "EmpresaFechaIngreso", "EmpresaNombre", "EmpresaSalario", "EmpresaTelefono", "FechaCreacion", "Nombres", "PasswordHash", "PasswordSalt", "Rol" },
                values: new object[] { 25, "000000000000", "8090090909", "admin@inverweb.com", "No tiene dirección", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0.0, null, new DateTime(2021, 11, 18, 22, 43, 40, 585, DateTimeKind.Local).AddTicks(5629), "Administrador InverWeb", new byte[] { 150, 217, 124, 160, 47, 124, 66, 40, 18, 30, 150, 190, 80, 140, 180, 58, 208, 90, 16, 54, 77, 73, 128, 92, 23, 173, 72, 139, 250, 112, 81, 175, 2, 133, 124, 240, 183, 229, 98, 71, 253, 83, 82, 121, 119, 47, 250, 127, 80, 245, 208, 109, 141, 163, 106, 186, 155, 253, 89, 153, 255, 65, 188, 185 }, new byte[] { 149, 94, 0, 101, 195, 137, 36, 24, 63, 32, 107, 124, 197, 235, 237, 33, 193, 119, 203, 253, 150, 223, 34, 154, 157, 122, 160, 214, 146, 171, 134, 127, 8, 109, 218, 59, 179, 13, 114, 14, 186, 183, 190, 106, 226, 66, 153, 164, 72, 135, 244, 171, 101, 213, 165, 143, 137, 153, 93, 106, 163, 195, 77, 77, 77, 198, 4, 208, 181, 192, 30, 164, 40, 129, 198, 16, 188, 121, 33, 243, 156, 57, 34, 17, 240, 141, 133, 84, 197, 63, 0, 87, 150, 16, 134, 7, 55, 5, 246, 135, 61, 220, 27, 209, 39, 235, 92, 170, 171, 169, 84, 167, 218, 172, 50, 190, 121, 170, 13, 123, 186, 182, 244, 50, 116, 222, 116, 69 }, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
