using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // FORM TƯ VẤN
        // =========================
        public IActionResult Consult(string carName)
        {
            ViewBag.CarName = carName;
            return View();
        }

        // =========================
        // KHÁCH GỬI FORM
        // =========================
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            customer.CreatedAt = DateTime.Now;

            customer.Status = "Mới";

            customer.Address ??= "";
            customer.Email ??= "";
            customer.Phone ??= "";
            customer.FullName ??= "";

            customer.InterestedCar ??= "";

            _context.Customers.Add(customer);

            _context.SaveChanges();

            TempData["success"] =
                "Cảm ơn bạn đã liên hệ, chúng tôi sẽ phản hồi sớm nhất.";

            return RedirectToAction("Index", "Home");
        }
    }
}
