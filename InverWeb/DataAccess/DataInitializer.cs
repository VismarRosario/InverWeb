using InverWeb.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace InverWeb.DataAccess
{
    public static class DataInitializer
    {
        public static void DataDePrueba(this ModelBuilder modelBuilder)
        {
            LogicaClaves.CrearClave("InverWeb@01", out byte[] ClaveHash, out byte[] ClaveSalt);

            Usuario usuario = new Usuario()
            {
                ID = 25,
                Nombres = "Administrador InverWeb",
                Cedula = "000000000000",
                Correo = "admin@inverweb.com",
                PasswordHash = ClaveHash,
                PasswordSalt = ClaveSalt,
                Celular = "8090090909",
                Direccion = "No tiene dirección",
                Rol = Rol.Administrador
            };

            modelBuilder.Entity<Usuario>().HasData(usuario);
        }
    }

}
