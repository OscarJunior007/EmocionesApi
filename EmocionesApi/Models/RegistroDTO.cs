using System.ComponentModel.DataAnnotations;

namespace EmocionesApi.Models
{
    public class RegistroDTO
    {
       

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [StringLength(50, ErrorMessage = "El correo no puede superar los 50 caracteres")]
        [EmailAddress(ErrorMessage = "Formato de correo invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

    }
}
