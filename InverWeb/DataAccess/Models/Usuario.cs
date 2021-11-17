using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InverWeb.DataAccess.Models
{
    public class Usuario
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(15)]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(60)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^\s\,]*$", ErrorMessage = "No se permiten espacios en blanco")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public byte[] PasswordHash { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public byte[] PasswordSalt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Solo se permiten números de 10 dígitos")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public Rol Rol { get; set; }

        // Perfil de la empresa
        [StringLength(50)]
        public string EmpresaNombre { get; set; }

        [StringLength(25)]
        public string EmpresaCargo { get; set; }

        [Range(1, double.MaxValue)]
        public double EmpresaSalario { get; set; }

        [StringLength(100)]
        public string EmpresaDireccion { get; set; }

        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Solo se permiten números de 10 dígitos")]
        public string EmpresaTelefono { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EmpresaFechaIngreso { get; set; }
    }

}
