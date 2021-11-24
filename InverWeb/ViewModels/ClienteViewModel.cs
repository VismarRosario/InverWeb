using System;
using System.ComponentModel.DataAnnotations;

namespace InverWeb.ViewModels
{
    public class ClienteViewModel
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
        [EmailAddress(ErrorMessage = "Este no es un Email Válido")]
        [RegularExpression(@"^[^\s\,]*$", ErrorMessage = "No se permiten espacios en blanco")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Solo se permiten números de 10 dígitos")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Direccion { get; set; }

        // Perfil de la empresa
        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(50)]
        public string EmpresaNombre { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(25)]
        public string EmpresaCargo { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Range(1, double.MaxValue)]
        public double EmpresaSalario { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(100)]
        public string EmpresaDireccion { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Solo se permiten números de 10 dígitos")]
        public string EmpresaTelefono { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.DateTime)]
        public DateTime EmpresaFechaIngreso { get; set; }
    }
}
