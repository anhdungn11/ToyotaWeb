using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;

namespace ToyotaWeb.Controllers
{
    public class RollingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RollingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var car = _context.Cars.FirstOrDefault(c => c.Slug == slug);

            if (car == null)
                return NotFound();

            return View(car);
        }
    }
}