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
            _db.Categoria.Add(categoria);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
