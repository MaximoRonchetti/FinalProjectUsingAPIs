using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Model;
using MiPrimeraApi.Repository;

namespace MiPrimeraApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoVendidoController : ControllerBase
    {
        [HttpGet(Name = "Traer Productos Vendidos")]
        public List<ProductoVendido> GetProductosVendidos(Usuario usuario)
        {
            List<ProductoVendido> productosVendidos = ProductoVendidoHandler.TraerProductosVendidos(usuario);
            return productosVendidos;
        }
    }
}
