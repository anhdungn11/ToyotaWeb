using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CarController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private string GenerateSlug(string text)
        {
            text = text.ToLower();

            text = text.Replace("à", "a")
                       .Replace("á", "a")
                       .Replace("ạ", "a")
                       .Replace("ả", "a")
                       .Replace("ã", "a")
                       .Replace("â", "a")
                       .Replace("ă", "a")
                       .Replace("đ", "d");

            text = text.Replace(" ", "-");

            return text;
        }

       
        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }
      

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
                return View(car);
                car.Slug = GenerateSlug(car.Name);

            if (car.ImageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(car.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await car.ImageFile.CopyToAsync(stream);
                }

                car.ImageUrl = "/uploads/" + fileName;
            }

            _context.Cars.Add(car);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car, IFormFile? imageFile)
        {
            if (id != car.CarId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(car);

            var existingCar = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.CarId == id);
            if (existingCar == null)
                return NotFound();

            
            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                car.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                
                car.ImageUrl = existingCar.ImageUrl;
            }
            car.Slug = GenerateSlug(car.Name);
            _context.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}