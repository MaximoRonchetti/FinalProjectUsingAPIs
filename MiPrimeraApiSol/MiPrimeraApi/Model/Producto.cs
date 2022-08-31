namespace MiPrimeraApi.Model
{
    public class Producto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public double Costo { get; set; }
        public double PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int IdUsuario { get; set; }
        public Producto(string descripcion, double costo, double precioVenta, int stock)
        {
            Descripcion = descripcion;
            Costo = costo;  
            PrecioVenta = precioVenta;
            Stock = stock;
        }

        public Producto()
        {

        }
    }
}
