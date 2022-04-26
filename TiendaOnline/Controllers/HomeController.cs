using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Models.ViewModels;
using TiendaOnline.Utilidades;

namespace TiendaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {               
                Productos = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion),
                Categorias = _db.Categoria
            };

            return View(homeVM);
        }

        public IActionResult Detalle(int Id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);
            }

            DetalleVM detalleVM = new DetalleVM()
            {
                Producto = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion)
                                        .Where(p => p.Id== Id).FirstOrDefault(),
                ExiteEnCarrito = false
            };

            foreach (var item in carroComprasLista)
            {
                if(item.ProductoId == Id)
                {
                    detalleVM.ExiteEnCarrito=true;
                }
            }

            return View(detalleVM);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>();

            if(HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);
            }
            carroComprasLista.Add(new CarroCompras() { ProductoId = Id});
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);
            }
           
            var productoARemover = carroComprasLista.SingleOrDefault(x => x.ProductoId == Id);
            
            if (productoARemover != null)
            {
                carroComprasLista.Remove(productoARemover);
            }

            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}