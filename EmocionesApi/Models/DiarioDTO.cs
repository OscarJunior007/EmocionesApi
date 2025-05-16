using System.ComponentModel.DataAnnotations;

namespace EmocionesApi.Models
{
    public class DiarioDTO
    {
        [MaxLength(200)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El Estado del animo es obligatorio")]

        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }

        public string EstadoAnimo { get; set; }
       
        public string Etiquetas { get; set; }

        public Guid idUser { get; set; }

    }
}
