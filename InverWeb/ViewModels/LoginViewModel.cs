using System.ComponentModel.DataAnnotations;

namespace InverWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(45)]
        [EmailAddress(ErrorMessage = "Este no es un Email Vlido")]
        [RegularExpression(@"^[^\s\,]*$", ErrorMessage = "No se permiten espacios en blanco")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
    }
}
