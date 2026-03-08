using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using System;

namespace ToyotaWeb.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== FORM USER GỬI =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            model.CreatedAt = DateTime.Now;
            model.IsCalled = false;

            _context.Contacts.Add(model);
            _context.SaveChanges();

            TempData["success"] = "Gửi yêu cầu thành công!";

            return RedirectToAction("Index", "Home");
        }
    }
}