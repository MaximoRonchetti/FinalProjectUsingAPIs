using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NombreController : ControllerBase
    {
        string nombre = "Proyecto Final C# Maximo Ronchetti";

        [HttpGet(Name = "Traer Nombre")]
        public string TraerNombre()
        {
            return nombre;
        }
    }
}
