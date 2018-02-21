using Microsoft.AspNetCore.Mvc;

namespace Sunflower.App.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}