using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Model;
using MiPrimeraApi.Repository;

namespace MiPrimeraApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class VentaController : ControllerBase
    {
        [HttpGet(Name = "Traer Ventas")]
        public List<Venta> GetVentas(Usuario usuario)
        {
            List<Venta> ventas = VentaHandler.TraerVentas(usuario);
            return ventas;
        }

        [HttpDelete(Name = "Eliminar Venta")]
        public bool DeleteVenta(int id)
        {
            bool ventaElimExistosa = VentaHandler.EliminarVenta(id);
            return ventaElimExistosa;
        }

        [HttpPost (Name = "Agregar Venta")]
        public bool AddVenta([FromBody]List<Producto> productos, int idUsuario)
        {
            bool ventaAggExistosa = VentaHandler.AgergarVenta(productos, idUsuario);
            return ventaAggExistosa;
        }

    }
}
