using System.ComponentModel.DataAnnotations;

namespace EmocionesApi.Models
{
    public class LoginDTO
    {



        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
