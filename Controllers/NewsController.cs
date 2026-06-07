using Microsoft.AspNetCore.Mvc;

namespace ToyotaWeb.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Products() => View();
        public IActionResult Promotions() => View();
        public IActionResult Social() => View();
        public IActionResult Support() => View();
    }
}