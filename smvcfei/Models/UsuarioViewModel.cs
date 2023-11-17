using System.ComponentModel.DataAnnotations;

namespace smvcfei.Models
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es correo válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        // Observe el atributo Compare. Compara contra otro campo que sus valores sean iguales
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Compare(otherProperty: "Password", ErrorMessage = "La contraseñas no coindicen.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme su contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
