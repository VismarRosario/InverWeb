using System.ComponentModel.DataAnnotations;

namespace InverWeb.ViewModels
{
    public class SuperAdminViewModel
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

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        [Compare("Clave", ErrorMessage = "Debe repetir la clave")]
        public string RepetirClave { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Solo se permiten números de 10 dígitos")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Direccion { get; set; }
    }
}
