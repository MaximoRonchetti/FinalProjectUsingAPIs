using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Model;
using MiPrimeraApi.Repository;

namespace MiPrimeraApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        [HttpGet(Name = "Traer Productos")]
        public List<Producto> GetProductos(Usuario usuario)
        {
            List<Producto> productos = ProductoHandler.TraerProductos(usuario);
            return productos;
        }

        [HttpDelete(Name = "Eliminar Producto")]
        public int DeleteProducto([FromBody] int id)
        {
            int productoElimExitosa = ProductoHandler.EliminarProducto(id);
            //productoElimExitosa = 0 (no se elimino nada), = 1 (elimino producto pero no productovendido) porque esta vendido, = 2 (se elimino todo)
            return productoElimExitosa;
        }

        [HttpPost (Name = "Agregar Producto")]
        public bool AddProducto(/*string nombreUsuario,*/ [FromBody]Producto producto)
        {
            //Usuario usuarioActual = UsuarioHandler.TraerUsuario(nombreUsuario);
            bool productoAggExitosa = ProductoHandler.CrearProducto(producto/*, usuarioActual*/);
            return productoAggExitosa;
        }

        [HttpPut (Name = "Modificar Producto")]
        public bool UpdateProducto([FromBody]Producto producto)
        {
            bool productoModExitosa = ProductoHandler.ModificarProducto(producto);
            return productoModExitosa;
        }

    }
}
