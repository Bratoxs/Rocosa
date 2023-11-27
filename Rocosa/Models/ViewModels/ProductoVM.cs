using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocosa.Models.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }

        //Lista para Categoría
        public IEnumerable<SelectListItem>? CategoriaLista { get; set; }

        //Lista para TipoAplicacion
        public IEnumerable<SelectListItem>? TipoAplicacionLista { get; set; }
    }
}
