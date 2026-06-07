using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;

namespace ToyotaWeb.Areas.Sale.Controllers
{
    [Area("Sale")]
    [Authorize(Roles = "Admin,Sale")]
    public class SaleOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleOrderController(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders =
                await _context.SaleOrders
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(orders);
        }
        // ================= DETAILS =================

        public async Task<IActionResult> Details(int id)
        {
            var order =
                await _context.SaleOrders
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // ================= UPDATE STATUS =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id,
            string status
        )
        {
            var order =
                await _context.SaleOrders
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = id }
            );
        }
        // ================= CREATE =================

        public async Task<IActionResult> Create(int customerId)
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

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    int customerId,
    string carName,
    decimal price,
    decimal deposit,
    string note
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

            // ================= INVENTORY =================

            var inventory =
                await _context.Inventories
                .FirstOrDefaultAsync(x =>
                    x.CarName == carName &&
                    x.Quantity > 0);

            if (inventory == null)
            {
                TempData["error"] =
                    "Xe hiện đã hết hàng.";

                return RedirectToAction(
                    "Create",
                    new { customerId = customerId });
            }

            // ================= CREATE ORDER =================

            var order =
                new ToyotaWeb.Models.SaleOrder
                {
                    CustomerName =
                        customer.FullName,

                    CarName = carName,

                    Price = price,

                    Deposit = deposit,

                    Note = note,

                    Status = "Pending",

                    CreatedDate =
                        DateTime.Now
                };

            _context.SaleOrders.Add(order);

            // ================= CUSTOMER =================

            customer.Status = "Đã cọc";

            // ================= INVENTORY =================

            inventory.Quantity--;

            if (inventory.Quantity <= 0)
            {
                inventory.Quantity = 0;

                inventory.Status = "Hết hàng";
            }

            await _context.SaveChangesAsync();

            TempData["success"] =
                "Tạo đơn hàng thành công.";

            return RedirectToAction("Index");
        }
    }
}