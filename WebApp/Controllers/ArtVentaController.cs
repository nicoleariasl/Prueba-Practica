using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using WebApp.Models;

namespace WebApp.Controllers
{
    
    public class ArtVentaController : BaseController
    {
        private readonly HttpClient _httpClient;

        public ArtVentaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7228/api");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/ArtVenta/ArtVenta");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var articulos = JsonConvert.DeserializeObject<IEnumerable<ArtVenta>>(content);
                return View("Index", articulos);
            }

            return View(new List<ArtVenta>());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArtVenta art)
        {
            if (art.Iva == true)
            {
                art.Total = (decimal?)((double.Parse(art.Precio.ToString()) * 0.13) + double.Parse(art.Precio.ToString()));
            }
            else
            {
                art.Iva = false;
                art.Total = art.Precio;
            }

            
                if (!art.Codigo.StartsWith("ART"))
                {
                    TempData["Error"] = "Es necesario que el código inicie con la abreviatura ART";
                    return RedirectToAction("Index");
                }
                else {
                    if (Regex.IsMatch(art.Codigo, "^ART\\d{2,}$"))
                    {
                        var json = JsonConvert.SerializeObject(art);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await _httpClient.PostAsync("/api/ArtVenta/crear", content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var responseData = JsonConvert.DeserializeAnonymousType(responseContent, new { mensaje = "" });
                            if (responseData.mensaje.Equals("Código existente."))
                            {
                                TempData["Error"] = responseData.mensaje;
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                Alert("Artículo guardado", NotificationType.success, "El nuevo artículo fue guardado correctamente.");
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            Alert("Ocurrió un error", NotificationType.error, "El artículo no pudo ser guardado, intentelo nuevamente.");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Es necesario que el código contenga al menos 2 números.";
                        return RedirectToAction("Index");
                    }
                }


        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArtVenta art)
        {
            if (art.Iva == true)
            {
                art.Total = (decimal?)((double.Parse(art.Precio.ToString()) * 0.13) + double.Parse(art.Precio.ToString()));
            }
            else
            {
                art.Iva = false;
                art.Total = art.Precio;
            }

            if (ModelState.IsValid)
            {
                if (!art.Codigo.StartsWith("ART"))
                {
                    TempData["Error"] = "Es necesario que el código inicie con la abreviatura ART";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (Regex.IsMatch(art.Codigo, "^ART\\d{2,}$"))
                    {
                        var json = JsonConvert.SerializeObject(art);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await _httpClient.PutAsync($"/api/ArtVenta/editar?id={art.Id}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var responseData = JsonConvert.DeserializeAnonymousType(responseContent, new { mensaje = "" });
                            if (!responseData.mensaje.Equals("okEdit"))
                            {
                                TempData["Error"] = responseData.mensaje;
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                Alert("El artículo fue actualizado correctamente.", NotificationType.success, "Artículo actualizado");
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            Alert("Ocurrió un error", NotificationType.error, "El artículo no pudo ser guardado, intentelo nuevamente.");
                            return RedirectToAction("Index");

                        }
                    }
                    else
                    {
                        TempData["Error"] = "Es necesario que el código contenga al menos 2 números.";
                        return RedirectToAction("Index");
                    }
                }

            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/ArtVenta/eliminar?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeAnonymousType(responseContent, new { mensaje = "" });
                if (!responseData.mensaje.Equals("okDelete"))
                {
                    TempData["Error"] = responseData.mensaje;
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("El artículo fue eliminado correctamente.", NotificationType.success, "Artículo eliminado");
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

    }
}
