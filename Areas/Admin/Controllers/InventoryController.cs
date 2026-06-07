using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================

        public IActionResult Index(
            string? keyword,
            string? status)
        {
            var query = _context.Inventories.AsQueryable();

            // SEARCH

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.CarName.Contains(keyword) ||
                    x.VinNumber.Contains(keyword));
            }

            // FILTER STATUS

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x =>
                    x.Status == status);
            }

            var inventories = query
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            ViewBag.Keyword = keyword;

            ViewBag.Status = status;

            return View(inventories);
        }

        // ================= CREATE =================

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inventory model)
        {
            if (ModelState.IsValid)
            {
                // AUTO STATUS

                if (model.Quantity <= 0)
                {
                    model.Status = "Hết hàng";
                }
                else
                {
                    model.Status = "Còn hàng";
                }

                _context.Inventories.Add(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }
        // ================= EDIT =================

        public IActionResult Edit(int id)
        {
            var inventory =
                _context.Inventories
                .FirstOrDefault(x => x.Id == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        [HttpPost]
        public IActionResult Edit(Inventory model)
        {
            var inventory =
                _context.Inventories
                .FirstOrDefault(x => x.Id == model.Id);

            if (inventory == null)
                return NotFound();

            inventory.CarName = model.CarName;
            inventory.Color = model.Color;
            inventory.VinNumber = model.VinNumber;
            inventory.Quantity = model.Quantity;
            inventory.ImportPrice = model.ImportPrice;
            inventory.SalePrice = model.SalePrice;
           if (model.Quantity <= 0)
            {
                inventory.Status = "Hết hàng";
            }
            else
            {
                inventory.Status = "Còn hàng";
            }
            inventory.Branch = model.Branch;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= DELETE =================

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var inventory =
                _context.Inventories
                .FirstOrDefault(x => x.Id == id);

            if (inventory == null)
                return NotFound();

            _context.Inventories.Remove(inventory);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}