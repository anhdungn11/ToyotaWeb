using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using System.Linq;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                                   .OrderByDescending(x => x.CreatedAt)
                                   .ToList();

            return View(contacts);
        }

        [HttpPost]
        public IActionResult MarkAsCalled(int id, string note)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null)
                return Json(new { success = false });

            contact.IsCalled = true;
            contact.CallNote = note;

            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}