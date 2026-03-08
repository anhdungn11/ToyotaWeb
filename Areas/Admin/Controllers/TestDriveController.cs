using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestDriveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestDriveController(ApplicationDbContext context)
        {
            _context = context;
        }
public async Task<IActionResult> Approve(int id)
{
    var item = await _context.TestDrives.FindAsync(id);
    if (item == null)
        return NotFound();

    item.IsProcessed = true;
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestDrives.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _context.TestDrives.FindAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.TestDrives.FindAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var data = await _context.TestDrives.FindAsync(id);
            if (data != null)
            {
                _context.TestDrives.Remove(data);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}