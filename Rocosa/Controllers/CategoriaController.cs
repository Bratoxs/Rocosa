using Microsoft.AspNetCore.Mvc;
using Rocosa.Datos;
using Rocosa.Models;

namespace Rocosa.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Método para listar todos los registros dentro de la tabla
        //Get
        public IActionResult Index()
        {
            //Para obtener una lista
            IEnumerable<Categoria> lista = _db.Categoria;

            return View(lista);
        }

        //Método para crear un registro en la tabla
        //Get
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoria categoria) //Recibe el modelo Categoria
        {
            if (ModelState.IsValid) //Si el modelo cumple con todas las validaciones de los campos
            {
                _db.Categoria.Add(categoria);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        //Método para editar un registro en la tabla
        //Get
        public IActionResult Editar(int? Id) //Para que acepte valores nulos se pone el ?
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var obj = _db.Categoria.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria) //Recibe el modelo Categoria
        {
            if (ModelState.IsValid) //Si el modelo cumple con todas las validaciones de los campos
            {
                _db.Categoria.Update(categoria);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        //Método para eliminar un registro en la tabla
        //Get
        public IActionResult Eliminar(int? Id) //Para que acepte valores nulos se pone el ?
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var obj = _db.Categoria.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoria categoria) //Recibe el modelo Categoria
        {
            if (categoria == null) //Si el modelo cumple con todas las validaciones de los campos
            {
                return NotFound();
            }
            _db.Categoria.Remove(categoria);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
