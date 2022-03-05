using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Data;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TipoAplicacionController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _db.TipoAplicacion;
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoAplicacion tipoAplicacion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            

            _db.TipoAplicacion.Add(tipoAplicacion);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //GET : Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();            

            var tipoAplicacion = _db.TipoAplicacion.Find(id);

            if (tipoAplicacion == null) return NotFound();          

            return View(tipoAplicacion);

        }


        //POST : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TipoAplicacion tipoApp)
        {
            if(tipoApp == null) return NotFound();
            if (!ModelState.IsValid) return View(tipoApp);

            _db.TipoAplicacion.Update(tipoApp);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        //GET : Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var tipoApp = _db.TipoAplicacion.Find(id);
            if (tipoApp == null) return NotFound();


            return View(tipoApp);
        }

        //POST : Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(TipoAplicacion tipoApp)
        {
            if (tipoApp == null) return NotFound();

            _db.TipoAplicacion.Remove(tipoApp);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
