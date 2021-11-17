using InverWeb.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InverWeb.DataAccess
{
    public class GlobalDBContext : DbContext
    {
        // Obtener configuración de base de datos
        public GlobalDBContext(DbContextOptions<GlobalDBContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.DataDePrueba();
        }
    }

}
