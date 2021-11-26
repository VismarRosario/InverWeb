using System.ComponentModel.DataAnnotations;

namespace InverWeb.ViewModels
{
    public class NuevaClaveViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Password)]
        [Compare("Clave", ErrorMessage = "Debe repetir la clave")]
        public string RepetirClave { get; set; }
    }
}
