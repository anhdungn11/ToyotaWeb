using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using Microsoft.AspNetCore.Authorization;
namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =============================
        // LIST
        // =============================
        public async Task<IActionResult> Index()
        {
            var data = await _context.Customers
                .Include(x => x.Sale)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(data);
        }

        // =============================
        // 🔥 DETAILS (THÊM CÁI NÀY)
        // =============================
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers
                .Include(x => x.Sale)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }
    }
}