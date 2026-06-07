using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;

namespace ToyotaWeb.Areas.Sale.Controllers
{
    [Area("Sale")]
    [Authorize(Roles = "Admin,Sale")]
    public class TestDriveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestDriveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= LIST =================

        public async Task<IActionResult> Index()
        {
            var data = await _context.TestDrives
                .Where(x => x.Status == "Approved")
                .OrderByDescending(x => x.TestDate)
                .ToListAsync();

            return View(data);
        }
    }
}