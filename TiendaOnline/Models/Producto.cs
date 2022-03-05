using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre del producto es requerido")]
        public string? NombreProducto { get; set; }


        [Required(ErrorMessage = "La descripcion corta es requerida")]
        public string? DescripcionCorta { get; set; }


        [Required(ErrorMessage = "La descripcion es requerida")]
        public string? DescripcionLarga { get; set; }


        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        [Required(ErrorMessage = "El precio del producto es requerido")]
        public double Precio { get; set; }


        public string? ImageUrl { get; set; }

        public int ? CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }


        public int TipoAplicacionId { get; set; }
        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }

    }
}
