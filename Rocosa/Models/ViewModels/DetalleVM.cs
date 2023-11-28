namespace Rocosa.Models.ViewModels
{
    public class DetalleVM
    {
        //Otra forma es inicializar la variable directamente en esta vista y no en el controlador para ahorrar código, por medio de un constructor.
        public DetalleVM()
        {
            Producto = new Producto();
        }

        public Producto Producto { get; set; }

        public bool ExisteEnCarro { get; set; }
    }
}
