using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace ToyotaWeb.Controllers
{
    public class CompareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompareController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id1, int? id2)
        {
            var cars = _context.Cars.ToList();

            ViewBag.Car1 = id1.HasValue 
                ? cars.FirstOrDefault(c => c.CarId == id1) 
                : null;

            ViewBag.Car2 = id2.HasValue 
                ? cars.FirstOrDefault(c => c.CarId == id2) 
                : null;

            return View(cars);
        }

        public IActionResult Add(int id)
        {
            return RedirectToAction("Index", new { id1 = id });
        }
    }
}