using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Model;
using MiPrimeraApi.Repository;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InicioSesionController : ControllerBase
    {

        [HttpGet(Name = "Iniciar Sesion")]
        public Usuario IniciarSesion(string nombreUsuario, string constraseña)
        {
            Usuario usuarioIniciado = UsuarioHandler.IniciarSesion(nombreUsuario, constraseña);
            return usuarioIniciado;
        }
    }
}
