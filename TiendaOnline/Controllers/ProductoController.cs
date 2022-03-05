using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Models.ViewModels;

namespace TiendaOnline.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext? _db;

        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion);
            return View(lista);
        }

        //GET : Insert and Edit
        public IActionResult Upsert(int? Id)
        {

           

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _db.Categoria.Select(c => new SelectListItem { Value= c.Id.ToString(), Text= c.NombreCategoria }),
                TipoAplicacionLista = _db.TipoAplicacion.Select(c => new SelectListItem { Value= c.Id.ToString(), Text= c.Nombre })
            };

            if (Id == null) return View(productoVM);

            productoVM.Producto = _db.Producto.Find(Id);
            if (productoVM.Producto == null) return NotFound();          

            return View(productoVM);            
           
        }


        //POST : Insert and Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Producto producto)
        {
            if (ModelState.IsValid)
            {
                if(producto?.Id == null)
                {
                    _db.Producto.Add(producto);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    _db.Producto.Update(producto);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(producto);
        }
    }
}
