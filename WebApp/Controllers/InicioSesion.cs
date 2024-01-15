using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WebApp.Models;
using WebApp.Services;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApp.Controllers
{
    public class InicioSesion : BaseController
    {
        private readonly HttpClient _httpClient;
        private IUserService _userService;

        public InicioSesion(IHttpClientFactory httpClientFactory, IUserService _userService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7228/api");
            this._userService = _userService;
        }
        public IActionResult Index()
        {
            string? activeSession = HttpContext.Session.GetString("usuario");
            if (activeSession != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario usuario)
        {
            var json = JsonConvert.SerializeObject(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Usuario/usuarios", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeAnonymousType(responseContent, new { mensaje = "", usuario = new Usuario() });

                if (responseData.mensaje == "ok")
                {
                    // Autenticación exitosa, almacenar el objeto de usuario completo en la sesión
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name,responseData.usuario.Nombre),
                                new Claim(ClaimTypes.Email,responseData.usuario.Usuario1),
                                new Claim(ClaimTypes.NameIdentifier, responseData.usuario.Id.ToString())
                            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("usuario", JsonSerializer.Serialize(responseData.usuario));
                    return View("../Home/Index");
                }
                else
                {
                    TempData["Error"] = responseData.mensaje;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Error al intentar autenticar.";
                return RedirectToAction("Index");
            }

        }


        public async Task<ActionResult> LogoutAction()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("Index");
        }
    }
}
