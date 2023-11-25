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
            if (ModelState.IsValid) //Si el modelo cumple con todas las validaciones de los campos
            {
                _db.TipoAplicacion.Add(tipoAplicacion);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(tipoAplicacion);
        }

        //Método para editar un registro en la tabla
        //Get
        public IActionResult Editar(int? Id) //Para que acepte valores nulos se pone el ?
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var obj = _db.TipoAplicacion.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion) //Recibe el modelo Categoria
        {
            if (ModelState.IsValid) //Si el modelo cumple con todas las validaciones de los campos
            {
                _db.TipoAplicacion.Update(tipoAplicacion);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(tipoAplicacion);
        }

        //Método para eliminar un registro en la tabla
        //Get
        public IActionResult Eliminar(int? Id) //Para que acepte valores nulos se pone el ?
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var obj = _db.TipoAplicacion.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion) //Recibe el modelo TipoAplicacion
        {
            if (tipoAplicacion == null) //Si el modelo cumple con todas las validaciones de los campos
            {
                return NotFound();
            }
            _db.TipoAplicacion.Remove(tipoAplicacion);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
