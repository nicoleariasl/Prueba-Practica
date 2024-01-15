using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{

    public enum NotificationType
    {
        error,
        success,
        warning,
        info
    }


    public abstract class BaseController : Controller
    {
        public void Alert(string message, NotificationType type, string title = "")
        {
            TempData["notification"] = $"Swal.fire('{title}','{message}','{type.ToString().ToLower()}')";
        }
    }
}
