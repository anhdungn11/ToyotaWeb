using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

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

        public IActionResult Index(
            string bodyType,
            int? seats,
            string fuelType,
            string origin)
        {
            var cars = _context.Cars.AsQueryable();

            // FILTER

            if (!string.IsNullOrEmpty(bodyType))
            {
                cars = cars.Where(x => x.BodyType == bodyType);
            }

            if (seats.HasValue)
            {
                cars = cars.Where(x => x.Seats == seats);
            }

            if (!string.IsNullOrEmpty(fuelType))
            {
                cars = cars.Where(x => x.FuelType == fuelType);
            }

            if (!string.IsNullOrEmpty(origin))
            {
                cars = cars.Where(x => x.Origin == origin);
            }

            // CHỈ XE ĐANG KINH DOANH

            cars = cars.Where(x => x.IsActive == true);

            return View(cars.ToList());
        }

        // ================= CHI TIẾT XE =================

        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var car = _context.Cars
                .FirstOrDefault(x => x.Slug == slug);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // ================= DỰ TOÁN =================

        public IActionResult Estimate(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var car = _context.Cars
                .FirstOrDefault(x => x.Slug == slug);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
    }
}