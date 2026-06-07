using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using Microsoft.AspNetCore.Authorization;
namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SaleOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // DANH SÁCH ĐƠN HÀNG
        // =========================
        public IActionResult Index()
        {
            var orders = _context.SaleOrders
                .Include(x => x.Sale)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            ViewBag.TotalRevenue = orders.Sum(x => x.Price);
            ViewBag.TotalDebt = orders.Sum(x => x.Debt);
            ViewBag.TotalOrders = orders.Count;

            return View(orders);
        }

        // =========================
        // FORM TẠO
        // =========================
        public IActionResult Create()
        {
            ViewBag.Sales = _context.Sales.ToList();

            return View();
        }

        // =========================
        // TẠO ĐƠN
        // =========================
        [HttpPost]
        public IActionResult Create(SaleOrder model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;

                _context.SaleOrders.Add(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Sales = _context.Sales.ToList();

            return View(model);
        }

        // =========================
        // FORM SỬA
        // =========================
        public IActionResult Edit(int id)
        {
            var order = _context.SaleOrders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Sales = _context.Sales.ToList();

            return View(order);
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPost]
        public IActionResult Edit(SaleOrder model)
        {
            if (ModelState.IsValid)
            {
                _context.SaleOrders.Update(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Sales = _context.Sales.ToList();

            return View(model);
        }

        // =========================
        // DELETE
        // =========================
        public IActionResult Delete(int id)
        {
            var order = _context.SaleOrders.Find(id);

            if (order != null)
            {
                _context.SaleOrders.Remove(order);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}