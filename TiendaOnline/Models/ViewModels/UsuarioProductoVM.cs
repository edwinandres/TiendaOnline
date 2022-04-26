namespace TiendaOnline.Models.ViewModels
{
    public class UsuarioProductoVM
    {

        public UsuarioProductoVM()
        {
            ListaProductos = new List<Producto>();
        }

        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        public IEnumerable<Producto> ListaProductos { get; set; }
    }
}
