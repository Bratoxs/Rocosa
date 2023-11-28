using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocosa.Datos;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using Rocosa.Utilidades;
using System.Diagnostics;

namespace Rocosa.Controllers
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
            //Llenar ViewModel
            HomeVM homeVM = new HomeVM()
            {
                Productos = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion),
                Categorias = _db.Categoria
            };

            return View(homeVM);
        }

        //Método para mostrar el detalle de cada producto y cargar al carrito de compras si ya existe agregado aparezaca el botón "Remover del carro"
        //Get
        public IActionResult Detalle(int Id)
        {
            //Validar el producto se encuentra agregado a nuestra sesión
            List<CarroCompra> carroComprasLista = new List<CarroCompra>(); //Variable tipo lista
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            DetalleVM detalleVM = new DetalleVM()
            {
                Producto = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion)
                                        .Where(p => p.Id == Id).FirstOrDefault(), //Tambien se puede mandar asi FirstOrDefault(p => p.Id == Id) y eliminar el Where
                ExisteEnCarro = false
            };

            //Recorrer para verificar que el producto se encuentra agregado
            foreach (var item in carroComprasLista)
            {
                if (item.ProductId == Id)
                {
                    detalleVM.ExisteEnCarro = true; //Indicamos que si existe
                }
            }

            return View(detalleVM);
        }

        //Método post para agregar producto al carrito de compras
        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>(); //Variable tipo lista
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            //Agregar el producto a nuestra sesión
            carroComprasLista.Add(new CarroCompra { ProductId = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista); //Llenamos la sesión

            return RedirectToAction(nameof(Index));
        }

        //Método para remover producto del carrito de compras
        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>(); //Variable tipo lista
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            
            var productoARemover = carroComprasLista.SingleOrDefault(x => x.ProductId == Id); //Validar si el producto esta agregado al carro de compras

            if (productoARemover != null)
            {
                carroComprasLista.Remove(productoARemover); //Si existe removemos el producto
            }

            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista); //Se actualiza la sesión

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}