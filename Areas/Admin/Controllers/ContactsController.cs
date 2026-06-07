using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var contacts = _context.Contacts
                .Include(c => c.Sale)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return View(contacts);
        }

        public IActionResult AssignSale(int id)
        {
            var contact = _context.Contacts
                .Include(c => c.Sale)
                .FirstOrDefault(c => c.Id == id);

            if (contact == null)
                return NotFound();

            ViewBag.Sales = new SelectList(
                _context.Sales.ToList(),
                "Id",
                "Name",
                contact.SaleId
            );

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignSale(int id, int? saleId)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
                return NotFound();

            contact.SaleId = saleId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult MarkAsCalled(int id, string? note)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
                return Json(new { success = false });

            contact.IsCalled = true;
            contact.CallNote = note;

            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
                return NotFound();

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}