# proyecto_final

# class Usuario
    {
        private int Id;
        private string Nombre;
        private string Apellido;
        private string NombreUsuario;
        private string Contrasenia;
        private string Mail;
    }

    class Producto
    {
        private int Id;
        private string Descrpcion;
        private double Costo;
        private double PrecioVenta;
        private int Stock;
        private int IdUsuario;
    }

    class ProductoVendido
    {
        private int Id;
        private int IdProducto;
        private int Stock;
        private int IdVenta;
    }

    class Venta
    {
        private int Id;
        private string Comentarios;
    }