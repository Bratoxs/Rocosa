using System.ComponentModel.DataAnnotations;

namespace Rocosa.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre de Categoria es Obligatorio.")]
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "Orden es Obligatorio.")]
        //Validar que en número ingresado es mayor a cero
        [Range(1, int.MaxValue, ErrorMessage = "El Orden debe de ser Mayor a Cero.")]
        public int MostrarOrden { get; set; }
    }
}
