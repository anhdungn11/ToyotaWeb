using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

    //     public IActionResult Index()
    //     {
    //         var cars = _context.Cars
    // .Include(c => c.CarVariants)
    //     .ThenInclude(v => v.Images)
    // .Where(c => c.IsActive == true)
    // .ToList();

    //         return View(cars);


    //     }
      public IActionResult Index(string? sortOrder)
{
    var cars = _context.Cars
        .Include(c => c.CarImages)
        .Where(c => c.IsActive);

    if (sortOrder == "price_asc")
    {
        cars = cars.OrderBy(c => c.CarVariants.Min(v => v.Price));
    }

    return View(cars.ToList());
}


public IActionResult Details(int id)
{
    var car = _context.Cars
        .Include(c => c.CarVariants)
            .ThenInclude(v => v.Images)
       .FirstOrDefault(c => c.CarId == id && c.IsActive == true);


    if (car == null)
        return NotFound();

    return View(car);
}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        
    }
}