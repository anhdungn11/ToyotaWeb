using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;

namespace ToyotaWeb.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize(Roles = "Admin,Accountant")]
    public class RevenueReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RevenueReportController(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        // ================= REPORT =================

        public IActionResult Index(
            int? month,
            int? year
        )
        {
            int currentMonth =
                month ?? DateTime.Now.Month;

            int currentYear =
                year ?? DateTime.Now.Year;

            // ================= ORDERS =================

            var orders =
                _context.SaleOrders
                .Where(x =>
                    x.CreatedDate.Month == currentMonth
                    &&
                    x.CreatedDate.Year == currentYear
                )
                .ToList();

            // ================= DATA =================

            decimal revenue =
                orders.Sum(x => x.Price);

            decimal commission =
                orders.Sum(x => x.SaleCommission);

            int totalCars =
                orders.Count();

            decimal expenses =
                _context.CompanyExpenses
                .Where(x =>
                    x.CreatedDate.Month == currentMonth
                    &&
                    x.CreatedDate.Year == currentYear
                )
                .Sum(x => (decimal?)x.Amount)
                ?? 0;

            decimal salaries =
                _context.EmployeeSalaries
                .Where(x =>
                    x.Month == currentMonth
                    &&
                    x.Year == currentYear
                )
                .Sum(x => (decimal?)x.NetSalary)
                ?? 0;

            decimal profit =
                revenue
                - expenses
                - salaries;

            // ================= VIEWBAG =================

            ViewBag.Month =
                currentMonth;

            ViewBag.Year =
                currentYear;

            ViewBag.Revenue =
                revenue;

            ViewBag.Commission =
                commission;

            ViewBag.TotalCars =
                totalCars;

            ViewBag.Expenses =
                expenses;

            ViewBag.Salaries =
                salaries;

            ViewBag.Profit =
                profit;

            ViewBag.Orders =
                orders;

            return View();
        }
    }
}