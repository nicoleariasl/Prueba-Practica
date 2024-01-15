using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturacionController : ControllerBase
    {
        public readonly PruebaPContext _dbContext;

        public FacturacionController(PruebaPContext _context)
        {
            _dbContext = _context;

        }

        [HttpGet("{codigo}")]
        public IActionResult ObtenerArticulo(string codigo)
        {

            var articulo = _dbContext.ArtVenta.FirstOrDefault(a => a.Codigo == codigo);
            if (articulo != null)
            {
                return Ok(new { art = articulo });

            }
            else
            {
                return NotFound(new { mensaje = "El código no se encuentra en el sistema, inténtelo de nuevo." });
            }
        }

        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult> CrearFactura(Encabezado factura)
        {

            try
            {
                await _dbContext.Encabezados.AddAsync(factura);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "okCreate"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ocurrió un error al generar la factura." });
            }
        }

        [HttpGet]
        [Route("Detalles")]
        public async Task<IActionResult> ObtieneModelo()
        {
            int lastId = _dbContext.Encabezados.Max(x => x.FacturaId);
            //Recupera todo el objeto de encabezado y los detalles asociados a él
            var facturaC = await _dbContext.Detalles
            .Include(e => e.IdFacturaNavigation)
            .Include(e => e.CodArticuloNavigation)
            .Where(e => e.IdFactura == lastId)
            .Select(e => new
            {
                e.Id,
                e.IdFactura,
                e.CodArticulo,
                e.Cantidad,
                e.Total,
                IdFacturaNavigation = new
                {
                    e.IdFacturaNavigation.FacturaId,
                    e.IdFacturaNavigation.Fecha,
                    ClienteNavigation = new
                    {
                        e.IdFacturaNavigation.ClienteNavigation.Id,
                        e.IdFacturaNavigation.ClienteNavigation.Nombre,
                        e.IdFacturaNavigation.ClienteNavigation.Usuario1
                    }
                },
                CodArticuloNavigation = new
                {
                    e.CodArticuloNavigation.Codigo,
                    e.CodArticuloNavigation.Nombre,
                    e.CodArticuloNavigation.Id,
                    e.CodArticuloNavigation.Iva,
                    e.CodArticuloNavigation.Precio,
                    e.CodArticuloNavigation.Total,
                }
            })
            .ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "okCreate", fact = facturaC });
        }
    }
}
