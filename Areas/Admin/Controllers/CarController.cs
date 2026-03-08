using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // INDEX
        // ======================
        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }

        // ======================
        // CREATE - GET
        // ======================
        public IActionResult Create()
        {
            return View();
        }

        // ======================
        // CREATE - POST
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car car)
        {
            if (!ModelState.IsValid)
                return View(car);

            _context.Cars.Add(car);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // EDIT - GET
        // ======================
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            return View(car);
        }

        // ======================
        // EDIT - POST
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Car car)
        {
            if (id != car.CarId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(car);

            try
            {
                _context.Update(car);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(e => e.CarId == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // DELETE - GET
        // ======================
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            return View(car);
        }

        // ======================
        // DELETE - POST
        // ======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}