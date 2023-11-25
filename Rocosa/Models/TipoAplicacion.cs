using System.ComponentModel.DataAnnotations;

namespace Rocosa.Models
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre del Tipo de Aplicación es obligatorio.")]
        public string Nombre { get; set; }
    }
}
