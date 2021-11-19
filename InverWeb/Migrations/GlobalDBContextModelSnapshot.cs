﻿// <auto-generated />
using System;
using InverWeb.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InverWeb.Migrations
{
    [DbContext(typeof(GlobalDBContext))]
    partial class GlobalDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InverWeb.DataAccess.Models.Usuario", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cedula")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Celular")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpresaCargo")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("EmpresaDireccion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("EmpresaFechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmpresaNombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("EmpresaSalario")
                        .HasColumnType("float");

                    b.Property<string>("EmpresaTelefono")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Rol")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            ID = 25,
                            Cedula = "000000000000",
                            Celular = "8090090909",
                            Correo = "admin@inverweb.com",
                            Direccion = "No tiene dirección",
                            EmpresaFechaIngreso = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmpresaSalario = 0.0,
                            FechaCreacion = new DateTime(2021, 11, 18, 22, 43, 40, 585, DateTimeKind.Local).AddTicks(5629),
                            Nombres = "Administrador InverWeb",
                            PasswordHash = new byte[] { 150, 217, 124, 160, 47, 124, 66, 40, 18, 30, 150, 190, 80, 140, 180, 58, 208, 90, 16, 54, 77, 73, 128, 92, 23, 173, 72, 139, 250, 112, 81, 175, 2, 133, 124, 240, 183, 229, 98, 71, 253, 83, 82, 121, 119, 47, 250, 127, 80, 245, 208, 109, 141, 163, 106, 186, 155, 253, 89, 153, 255, 65, 188, 185 },
                            PasswordSalt = new byte[] { 149, 94, 0, 101, 195, 137, 36, 24, 63, 32, 107, 124, 197, 235, 237, 33, 193, 119, 203, 253, 150, 223, 34, 154, 157, 122, 160, 214, 146, 171, 134, 127, 8, 109, 218, 59, 179, 13, 114, 14, 186, 183, 190, 106, 226, 66, 153, 164, 72, 135, 244, 171, 101, 213, 165, 143, 137, 153, 93, 106, 163, 195, 77, 77, 77, 198, 4, 208, 181, 192, 30, 164, 40, 129, 198, 16, 188, 121, 33, 243, 156, 57, 34, 17, 240, 141, 133, 84, 197, 63, 0, 87, 150, 16, 134, 7, 55, 5, 246, 135, 61, 220, 27, 209, 39, 235, 92, 170, 171, 169, 84, 167, 218, 172, 50, 190, 121, 170, 13, 123, 186, 182, 244, 50, 116, 222, 116, 69 },
                            Rol = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
