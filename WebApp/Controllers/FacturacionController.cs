using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using WebApp.Models;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class FacturacionController : BaseController
    {
        private readonly HttpClient _httpClient;
        public FacturacionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7228/api");

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarFactura([FromBody] Encabezado factura)
        {
            try
            {
                // Validar facturaViewModel y realizar cualquier lógica adicional necesaria
                string? activeSession = HttpContext.Session.GetString("usuario");
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(activeSession);
                DateTime fechaHora = DateTime.Now;
                int userId = usuario.Id;
                factura.Cliente = userId;
                factura.Fecha = fechaHora;
                // Hacer una llamada a la API para guardar la factura
                var json = JsonConvert.SerializeObject(factura);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Facturacion/crear", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeAnonymousType(responseContent, new { mensaje = ""});
                if (responseData.mensaje.Equals("okCreate"))
                {
                   
                    return Json(new { success = true, message = "Factura generada exitosamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Error al generar la factura." });
                }



            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al generar la factura." });

            }
        }
       
        public async Task<IActionResult> GenerarPdf()
        {
            var response = await _httpClient.GetAsync("api/Facturacion/Detalles");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserializa el contenido JSON en un objeto anónimo
            var responseData = JsonConvert.DeserializeAnonymousType(responseContent, new { mensaje = "", fact = new List<Detalle>() });

            // Accede a la propiedad 'fact'
            var factura = responseData.fact;

            return new ViewAsPdf("GuardarFactura", factura)
            {
                FileName = $"Factura {factura.First().IdFactura}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };


        }
    }
}
