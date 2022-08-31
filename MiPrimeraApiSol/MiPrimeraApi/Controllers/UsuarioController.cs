using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Model;
using MiPrimeraApi.Repository;

namespace MiPrimeraApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsuarioController : ControllerBase
    {
        //Usuario usuarioActual;

        [HttpGet (Name = "Conseguir Usuario")]
        public Usuario GetUsuario(string nombreUsuario)
        {
            Usuario usuarioActual = UsuarioHandler.TraerUsuario(nombreUsuario);
            return usuarioActual;
        }

        [HttpDelete(Name = "Eliminar Usuario")]
        public bool DeleteUsuario([FromBody] int id)
        {
            bool usuarioElimExitosa = UsuarioHandler.EliminarUsuario(id);
            return usuarioElimExitosa;
        }

        [HttpPost(Name = "Agregar Usuario")]
        public bool AddUsuario([FromBody] Usuario usuario)
        {
            bool usuarioAggExitosa = UsuarioHandler.CrearUsuario(usuario);
            return usuarioAggExitosa;
        }

        [HttpPut (Name = "Actualizar Usuario")]
        public bool UpdateUsuario(Usuario usuario)
        {
            //Usuario usuarioActual = UsuarioHandler.TraerUsuario(nombreUsuario);
            bool usuarioModExitosa = UsuarioHandler.ModificarUsuario(usuario/*usuarioActual |nuevoNombre, nuevoApellido, nuevoNombreUsuario, nuevoConstraseña, nuevoMail*/);
            return usuarioModExitosa;
        }
    }
}
