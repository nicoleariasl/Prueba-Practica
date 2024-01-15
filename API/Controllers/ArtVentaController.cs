using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtVentaController : ControllerBase
    {
        public readonly PruebaPContext _dbContext;

        public ArtVentaController(PruebaPContext _context)
        {
            _dbContext = _context;

        }

        [HttpGet]
        [Route("ArtVenta")]
        public async Task<ActionResult<IEnumerable<ArtVenta>>> Listado() {

            var articulos = await _dbContext.ArtVenta.ToListAsync();
            return Ok(articulos);

        }

        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult> CrearArticulo(ArtVenta articulo)
        {
            var codExist = await _dbContext.ArtVenta.FirstOrDefaultAsync(c => c.Codigo == articulo.Codigo);

            if (codExist == null)
            {
                await _dbContext.ArtVenta.AddAsync(articulo);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "okCreate" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Código existente."});
            }
            
           
        }

        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> ActualizarArticulo(int id, ArtVenta articulo)
        {
            var articuloExistente = await _dbContext.ArtVenta.FindAsync(id);
            if(articuloExistente == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El artículo que intenta editar no se encuentra en el sistema." });
            }
            else
            {
                articuloExistente.Codigo = articulo.Codigo;
                articuloExistente.Nombre = articulo.Nombre;
                articuloExistente.Precio = articulo.Precio;
                articuloExistente.Iva = articulo.Iva;
                articuloExistente.Total = articulo.Total;

                await _dbContext.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "okEdit" });

            }
        }

        [HttpDelete]
        [Route("eliminar")]
        public async Task<IActionResult> EliminaArticulo(int id)
        {
            var findArt = await _dbContext.ArtVenta.FindAsync(id);

            if(findArt == null){
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El artículo que intenta eliminar no se encuentra en el sistema." });
            }else{
                var registrosRelacionados = await _dbContext.Detalles.Where(x => x.CodArticulo == id).ToListAsync();
                if (registrosRelacionados != null)
                {
                    _dbContext.Detalles.RemoveRange(registrosRelacionados);
                    _dbContext.ArtVenta.Remove(findArt);
                    await _dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "okDelete" });
                }
                else
                {
                    _dbContext.ArtVenta.Remove(findArt);
                    await _dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "okDelete" });
                }
            }
        }
    }
}
