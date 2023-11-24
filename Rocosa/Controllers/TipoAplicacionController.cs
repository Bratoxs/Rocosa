using Microsoft.AspNetCore.Mvc;
using Rocosa.Datos;
using Rocosa.Models;

namespace Rocosa.Controllers
{
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TipoAplicacionController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Método para listar todos los registros dentro de la tabla
        //Get
        public IActionResult Index()
        {
            //Para obtener una lista
            IEnumerable<TipoAplicacion> lista = _db.TipoAplicacion;

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
        public IActionResult Crear(TipoAplicacion tipoAplicacion) //Recibe el modelo Categoria
        {
            _db.TipoAplicacion.Add(tipoAplicacion);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
