namespace TiendaOnline.Models.ViewModels
{
    public class DetalleVM
    {

        public DetalleVM()
        {
            Producto Producto = new Producto();
        }

        public Producto Producto { get; set; }
        public bool ExiteEnCarrito { get; set; }
    }
}
