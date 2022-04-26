using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Models.ViewModels;
using TiendaOnline.Utilidades;

namespace TiendaOnline.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]

        public UsuarioProductoVM usuarioProductoVM { get; set; }

        public CarroController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<CarroCompras> carroComprasList = new List<CarroCompras>();

            if(HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null && HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() >0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);
            }

            List<int> prodEnCarro = carroComprasList.Select(i => i.ProductoId).ToList();
            IEnumerable<Producto> productList = _db.Producto.Where(p => prodEnCarro.Contains(p.Id));


            return View(productList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Resumen));
        }

        public IActionResult Resumen()
        {
            //Traer el usuario logueado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //
            List<CarroCompras> carroComprasList = new List<CarroCompras>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null && HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);
            }

            List<int> prodEnCarro = carroComprasList.Select(i => i.ProductoId).ToList();
            IEnumerable<Producto> productList = _db.Producto.Where(p => prodEnCarro.Contains(p.Id));

            usuarioProductoVM = new UsuarioProductoVM
            {
                ListaProductos = productList,
                UsuarioAplicacion = _db.UsuarioAplicacion.FirstOrDefault(u => u.Id == claim.Value)
            };

            return View(usuarioProductoVM);

        }

        public IActionResult Remover(int Id)
        {
            List<CarroCompras> carroComprasList = new List<CarroCompras>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null && HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);
            }

            carroComprasList.Remove(carroComprasList.FirstOrDefault(p => p.ProductoId == Id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasList);



            return RedirectToAction(nameof(Index));
        }
    }
}
