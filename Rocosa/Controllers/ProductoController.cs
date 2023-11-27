using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocosa.Datos;
using Rocosa.Models;
using Rocosa.Models.ViewModels;

namespace Rocosa.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment; //Esta variable nos permite el manejo de imagenes, desde la vista al controlador

        public ProductoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        //Método para listar todos los registros dentro de la tabla
        //Get
        public IActionResult Index()
        {
            //Con el Include trae los datos de sus tablas relacionadas
            IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria)
                                                      .Include(t => t.TipoAplicacion);
            return View(lista);
        }

        //Método para crear y editar un registro en la tabla, si recibe el Id edita el registro, si no recibe el Id se crea el registro
        //Get
        public IActionResult Upsert(int? Id) //Para que acepte valores nulos se pone el ?
        {
            /*Existe 2 forma de llenar un comboBox*/

            /*1. Usando un SelectList*/
            /*Crear una lista de tipo SelectList, para posterior llenar el combo de Categoría
            IEnumerable<SelectListItem> categoriaDropDown = _db.Categoria.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });

            ViewBag nos ayuda a trasferir información desde controlador a la vista
            ViewBag.categoriaDropDown = categoriaDropDown;

            Producto producto = new Producto(); //Objeto */

            /*2. Usando ViewModel*/
            //Inicializo mi ViewModel ProductoVM, cargar listas
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _db.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),
                TipoAplicacionLista = _db.TipoAplicacion.Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                })
            };


            if (Id == null)
            {
                //Crear un nuevo producto
                return View(productoVM);
            }
            else
            {
                //Editar un producto
                productoVM.Producto = _db.Producto.Find(Id);

                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM) //Recibe el ViewModel que viene de la vista
        {
            if (ModelState.IsValid) //Si el modelo cumple con todas las validaciones de los campos
            {
                //Trabajar con la imagen cargada en la vista
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {
                    //Crear
                    string upload = webRootPath + WC.ImagenRuta; //Conseguir la ruta
                    string fileName = Guid.NewGuid().ToString(); //Le asigne un ID automatico a la imagen que se va a guardar
                    string extension = Path.GetExtension(files[0].FileName); //Obtener la extesión de la imagen

                    //Recorrer con un using todos los bit que tenga la imagen, y se graba
                    //Se manda un files[0] ya que es un arreglo, y como siempre va hacer una imagen se carga la posición cero
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    //Grabar la imagen en el campo ImagenUrl, razón por la que se puso que este campo no sea requerido, porque es aqui donde recién se guarda
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    _db.Producto.Add(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    var objProducto = _db.Producto.AsNoTracking().FirstOrDefault(p => p.Id == productoVM.Producto.Id); //Trae todo el producto con su Id

                    if (files.Count > 0) //Si se esta cargando una nueva imagen
                    {
                        //Obtener ruta y extensión de la nueva imagen
                        string upload = webRootPath + WC.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //**Borrar la imagen anterior**
                        var anteriorFile = Path.Combine(upload, objProducto.ImagenUrl);

                        if (System.IO.File.Exists(anteriorFile)) //Verificar si esa imagen existe en el servidor
                        {
                            System.IO.File.Delete(anteriorFile); //Borra la imagen
                        }
                        //**Fin Borrar la imagen anterior**

                        //Crea la nueva imagen
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        //Grabar la imagen en el campo ImagenUrl
                        productoVM.Producto.ImagenUrl = fileName + extension;
                    }
                    else //Caso contrario si no carga una nueva imagen
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl; //Se mantiene con la misma imagen
                    }

                    //Actualizar el producto
                    _db.Producto.Update(productoVM.Producto);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            } // If ModelIsValid

            //Si algo falla volver a llenar las vistas
            productoVM.CategoriaLista = _db.Categoria.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });
            productoVM.TipoAplicacionLista = _db.TipoAplicacion.Select(t => new SelectListItem
            {
                Text = t.Nombre,
                Value = t.Id.ToString()
            });

            return View(productoVM);
        }



        //Método para eliminar un registro en la tabla
        //Get
        public IActionResult Eliminar(int? Id) //Para que acepte valores nulos se pone el ?
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }

            Producto producto = _db.Producto.Include(c => c.Categoria)
                                             .Include(t => t.TipoAplicacion)
                                             .FirstOrDefault(p => p.Id == Id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto) //Recibe el ViewModel que viene de la vista
        {
            if (producto == null)
            {
                return NotFound();
            }

            //Borrar la imagen
            string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta; //Ruta de la imagen
            var anteriorFile = Path.Combine(upload, producto.ImagenUrl);

            if (System.IO.File.Exists(anteriorFile)) //Verificar si esa imagen existe en el servidor
            {
                System.IO.File.Delete(anteriorFile); //Borra la imagen
            }

            _db.Producto.Remove(producto); //Eliminar el producto
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
