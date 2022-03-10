using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Models.ViewModels;

namespace TiendaOnline.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext? _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                //nuevo => add
                if (productoVM.Producto.Id == 0)
                {
                    //Guardar la imagen
                    string upload = webRootPath + WC.ImagenRuta;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    //Asignar la ruta de la imagen guardada a la propiedad imageUrl
                    productoVM.Producto.ImageUrl = filename + extension;
                    _db.Producto.Add(productoVM.Producto);
                    
                }
                //antiguo => update
                else
                {
                    var objProducto = _db.Producto.AsNoTracking().FirstOrDefault(p => p.Id == productoVM.Producto.Id);

                    //Si el usuario esta cargando una nueva imagen
                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagenRuta;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //borrar la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.ImageUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImageUrl = filename + extension;
                       
                    }
                    else
                    {
                        productoVM.Producto.ImageUrl = objProducto.ImageUrl;
                    }

                    _db.Producto.Update(productoVM.Producto);
                }
                //Guardar cambios tanto para insert como update
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Llenar nuevamentes las listas por si algo falla
            productoVM.CategoriaLista = _db.Categoria.Select(c => new SelectListItem { Value= c.Id.ToString(), Text= c.NombreCategoria });
            productoVM.TipoAplicacionLista = _db.TipoAplicacion.Select(c => new SelectListItem { Value= c.Id.ToString(), Text= c.Nombre });

            return View(productoVM);
            
        }

        //GET : Delete
        public IActionResult Delete(int? Id)
        {
            if (Id== null || Id == 0) return NotFound();

            Producto producto = _db.Producto.Include(p => p.Categoria).Include(p => p.TipoAplicacion).FirstOrDefault(p => p.Id == Id);
            if (producto!=null) return View(producto);

            return NotFound();
        }

        //POST : Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Producto producto)
        {
            if(producto == null) return NotFound();

            //Si tiene imagen , se elimina
            if(producto.ImageUrl != null)
            {
                string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta;
                var anteriorFile = Path.Combine(upload, producto.ImageUrl);
                if (System.IO.File.Exists(anteriorFile))
                {
                    System.IO.File.Delete(anteriorFile);
                }
            }
            

            //Eliminar el producto
            _db.Producto.Remove(producto);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
