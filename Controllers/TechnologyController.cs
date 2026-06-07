using Microsoft.AspNetCore.Mvc;

namespace ToyotaWeb.Controllers
{
    public class TechnologyController : Controller
    {
        public IActionResult Hybrid() => View();
        public IActionResult TSS() => View();
        public IActionResult TNGA() => View();
    }
}