using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using Microsoft.AspNetCore.Authorization;
namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TaxController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaxController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.SaleOrders.ToList();

            decimal totalRevenue = orders.Sum(x => x.Price);

            decimal taxRate = 0;

            if (totalRevenue <= 3000000000)
            {
                taxRate = 0.15m;
            }
            else if (totalRevenue <= 50000000000)
            {
                taxRate = 0.17m;
            }
            else
            {
                taxRate = 0.20m;
            }

            ViewBag.TotalRevenue = totalRevenue;

            ViewBag.TaxRate = taxRate * 100;

            ViewBag.TotalTax = totalRevenue * taxRate;

            ViewBag.TotalCars = orders.Count;

            return View(orders);
        }
    }
}