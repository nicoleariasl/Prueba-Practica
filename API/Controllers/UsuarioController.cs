using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        public readonly PruebaPContext _dbContext;

        public UsuarioController(PruebaPContext _context)
        {
            _dbContext = _context;
        }

        [HttpPost]
        [Route("usuarios")]
        public async Task<IActionResult> LoginAction(Usuario us)
        {
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(F => F.Usuario1 == us.Usuario1);
            try
            {
                if (usuario != null)
                {
                    if (usuario.Clave.Equals(us.Clave))
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", usuario = usuario });

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "Clave incorrecta" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Por favor, ingrese un correo válido" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex });
            }

            
        }
    }
}
