using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;

namespace ToyotaWeb.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= DANH SÁCH XE =================
        public IActionResult Index(string bodyType, int? seats, string fuelType, string origin)
        {
            var cars = _context.Cars
                .Include(c => c.CarImages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(bodyType))
                cars = cars.Where(c => c.BodyType == bodyType);

            if (seats.HasValue)
                cars = cars.Where(c => c.Seats == seats.Value);

            if (!string.IsNullOrEmpty(fuelType))
                cars = cars.Where(c => c.FuelType == fuelType);

            if (!string.IsNullOrEmpty(origin))
                cars = cars.Where(c => c.Origin == origin);

            return View(cars.ToList());
        }

        // ================= CHI TIẾT XE =================
        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var car = _context.Cars
                .Include(c => c.CarImages)
                .Include(c => c.CarVariants)
                .FirstOrDefault(c => c.Slug == slug);

            if (car == null)
                return NotFound();

            // 🔥 LẤY XE LIÊN QUAN (CÙNG BODYTYPE)
            var relatedCars = _context.Cars
                .Include(c => c.CarImages)
                .Where(c => c.CarId != car.CarId &&
                            c.BodyType == car.BodyType)
                .Take(4)
                .ToList();

            ViewBag.RelatedCars = relatedCars;

            return View(car);
        }

        // ================= TÍNH GIÁ =================
        public IActionResult Estimate(int id)
        {
            var car = _context.Cars
                .Include(c => c.CarVariants)
                .FirstOrDefault(c => c.CarId == id);

            if (car == null)
                return NotFound();

            return View(car);
        }
    }
}