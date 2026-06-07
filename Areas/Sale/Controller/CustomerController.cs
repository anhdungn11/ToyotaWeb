using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Sale.Controllers
{
    [Area("Sale")]
    [Authorize(Roles = "Admin,Sale")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        // ================= LIST =================

        public async Task<IActionResult> Index()
        {
            var customers =
                await _context.Customers
                .Include(x => x.Sale)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(customers);
        }

        // ================= DETAILS =================

        public async Task<IActionResult> Details(int id)
        {
            var customer =
                await _context.Customers
                .Include(x => x.Sale)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // ================= CRM TIMELINE =================

            var interactions =
                await _context.CustomerInteractions
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.CustomerId == id)
                .ToListAsync();

            ViewBag.Interactions =
                interactions;

            // ================= TEST DRIVE =================

            var testDrives =
                await _context.TestDrives
                .Where(x =>
                    x.Phone == customer.Phone
                )
                .OrderByDescending(x => x.TestDate)
                .ToListAsync();

            ViewBag.TestDrives =
                testDrives;

            // ================= SALE ORDERS =================

            var orders =
                await _context.SaleOrders
                .Where(x =>
                    x.CustomerName == customer.FullName
                )
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            ViewBag.Orders =
                orders;

            return View(customer);
        }

        // ================= ADD INTERACTION =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInteraction(
    int customerId,
    string content,
    string type,
    DateTime? nextFollowUpDate
)
        {
            // ================= VALIDATE =================

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["error"] =
                    "Vui lòng nhập nội dung chăm sóc khách hàng";

                return RedirectToAction(
                    "Details",
                    new { id = customerId }
                );
            }

            var interaction =
                new CustomerInteraction
                {
                    CustomerId = customerId,

                    Content = content,

                    Type = type,

                    CreatedAt = DateTime.Now,

                    NextFollowUpDate =
                        nextFollowUpDate
                };

            _context.CustomerInteractions
                .Add(interaction);

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Đã lưu CRM khách hàng";

            return RedirectToAction(
                "Details",
                new { id = customerId }
            );
        }

        // ================= UPDATE STATUS =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int customerId,
            string status
        )
        {
            var customer =
                await _context.Customers
                .FirstOrDefaultAsync(x =>
                    x.Id == customerId
                );

            if (customer == null)
            {
                return NotFound();
            }

            customer.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = customerId }
            );
        }
    }
}