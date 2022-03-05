using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Models
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }

    }
}
