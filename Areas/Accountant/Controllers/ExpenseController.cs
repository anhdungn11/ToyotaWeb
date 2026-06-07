using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize(Roles = "Admin,Accountant")]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= DANH SÁCH =================

        public IActionResult Index()
        {
            var expenses =
                _context.CompanyExpenses
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return View(expenses);
        }

        // ================= CREATE =================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CompanyExpense model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;

                _context.CompanyExpenses.Add(model);

                _context.SaveChanges();

                TempData["success"] =
                    "Thêm chi phí thành công";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }

        // ================= EDIT =================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var expense =
                _context.CompanyExpenses
                .FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CompanyExpense model)
        {
            try
            {
                var expense =
                    _context.CompanyExpenses
                    .FirstOrDefault(x => x.Id == model.Id);

                if (expense == null)
                {
                    return NotFound();
                }

                expense.ExpenseName =
                    model.ExpenseName;

                expense.Category =
                    model.Category;

                expense.Amount =
                    model.Amount;

                _context.SaveChanges();

                TempData["success"] =
                    "Cập nhật thành công";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }// ================= DELETE =================

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var expense =
                _context.CompanyExpenses
                .FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var expense =
                _context.CompanyExpenses
                .FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            _context.CompanyExpenses.Remove(expense);

            _context.SaveChanges();

            TempData["success"] =
                "Xóa thành công";

            return RedirectToAction("Index");
        }
    }
}