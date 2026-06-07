using Microsoft.AspNetCore.Mvc;

namespace ToyotaWeb.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult AfterSale() => View();
        public IActionResult Finance() => View();
        public IActionResult Insurance() => View();
        public IActionResult UsedCar() => View();
        public IActionResult Warranty() => View();
    }
}