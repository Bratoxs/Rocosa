using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del Producto es requerido.")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "Descripción Corta es requerida.")]
        public string DescripcionCorta { get; set; }

        [Required(ErrorMessage = "Descripción del Producto es requerido.")]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "El Precio del Producto es requerido.")]
        [Range(1, double.MaxValue, ErrorMessage = "El Precio debe ser Mayor a Cero.")]
        public double Precio { get; set; }

        public string? ImagenUrl { get; set; } //Se coloca ? para indicar que el dato no sea requerido

        //Foreign Key de la Tabla Categoria
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        //Foreign Key de la Tabla TipoAplicacion
        public int TipoAplicacionId { get; set; }

        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }
    }
}
