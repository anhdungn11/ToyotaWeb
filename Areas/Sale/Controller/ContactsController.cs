using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Sale.Controllers
{
    [Area("Sale")]
    [Authorize(Roles = "Sale")]

    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ContactsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var email = User.Identity.Name;

            var sale = _context.Sales
                .FirstOrDefault(x => x.Email == email);

            if (sale == null)
                return NotFound();

            var contacts = _context.Contacts
                .Where(x => x.SaleId == sale.Id)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(contacts);
        }

        [HttpPost]
        public IActionResult MarkAsCalled(int id, string? note)
        {
            var contact = _context.Contacts
                .FirstOrDefault(x => x.Id == id);

            if (contact == null)
                return Json(new { success = false });

            contact.IsCalled = true;

            contact.CallNote = note;

            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}