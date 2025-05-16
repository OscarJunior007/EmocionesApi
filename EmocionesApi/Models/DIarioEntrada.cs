using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmocionesApi.Models
{
    public class DIarioEntrada
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string EstadoAnimo { get; set; } 

        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now;

        public string Etiquetas { get; set; }


        // Clave foránea
        public Guid UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public User Usuario { get; set; }
    }
}
