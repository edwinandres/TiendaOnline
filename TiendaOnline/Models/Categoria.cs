using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string? NombreCategoria { get; set; }

        [Required(ErrorMessage = "El orden es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El orden debe ser mayor a cero.")]
        public int MostrarOrden { get; set; }



    }
}
