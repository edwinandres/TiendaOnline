using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Data;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Categoria> lista = _db.Categoria;
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _db.Categoria.Add(categoria);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
            
        }

        //GET : Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();           

            Categoria categoria = _db.Categoria.Find(id);

            if(categoria == null) return NotFound();
          
            return View(categoria);
        }


        //POST : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _db.Categoria.Update(categoria);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // GET : Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Categoria categoria = _db.Categoria.Find(id);

            if (categoria == null) return NotFound();

            return View(categoria);
        }


        //POST : Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Categoria categoria)
        {
            if(categoria == null) return NotFound();          

            _db.Categoria.Remove(categoria);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));               
        }
    }
}
