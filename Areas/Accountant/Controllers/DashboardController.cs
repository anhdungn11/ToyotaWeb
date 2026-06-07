using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using System.Text.Json;

namespace ToyotaWeb.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize(Roles = "Admin,Accountant")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        // ================= DASHBOARD =================

        public IActionResult Index()
        {
            // ================= THỜI GIAN =================

            int currentMonth =
                DateTime.Now.Month;

            int currentYear =
                DateTime.Now.Year;

            // ================= DOANH THU =================

            decimal totalRevenue =
                _context.SaleOrders
                .Where(x =>
                    x.CreatedDate.Month == currentMonth
                    &&
                    x.CreatedDate.Year == currentYear
                )
                .Sum(x => (decimal?)x.Price)
                ?? 0;

            // ================= CHI PHÍ =================

            decimal totalExpense =
                _context.CompanyExpenses
                .Where(x =>
                    x.CreatedDate.Month == currentMonth
                    &&
                    x.CreatedDate.Year == currentYear
                )
                .Sum(x => (decimal?)x.Amount)
                ?? 0;

            // ================= TỔNG LƯƠNG =================

            decimal totalSalary =
                _context.EmployeeSalaries
                .Where(x =>
                    x.Month == currentMonth
                    &&
                    x.Year == currentYear
                )
                .Sum(x => (decimal?)x.NetSalary)
                ?? 0;

            // ================= LỢI NHUẬN =================

            decimal netProfit =
                totalRevenue
                - totalExpense
                - totalSalary;

            // ================= XE ĐÃ BÁN =================

            int totalCarsSold =
                _context.SaleOrders
                .Count(x =>
                    x.CreatedDate.Month == currentMonth
                    &&
                    x.CreatedDate.Year == currentYear
                );

            // ================= VIEWBAG =================

            ViewBag.TotalRevenue =
                totalRevenue;

            ViewBag.TotalExpense =
                totalExpense;

            ViewBag.TotalSalary =
                totalSalary;

            ViewBag.NetProfit =
                netProfit;

            ViewBag.TotalCarsSold =
                totalCarsSold;

            // ================= CHART DATA =================

            List<string> months =
                new List<string>();

            List<decimal> revenues = new List<decimal>();

            List<decimal> expenses =
                new List<decimal>();

            List<decimal> profits =
                new List<decimal>();

            for (int i = 1; i <= 12; i++)
            {
                months.Add("Tháng " + i);

                // ================= REVENUE =================

                decimal revenue =
                    _context.SaleOrders
                    .Where(x =>
                        x.CreatedDate.Month == i
                        &&
                        x.CreatedDate.Year == currentYear
                    )
                    .Sum(x => (decimal?)x.Price)
                    ?? 0;

                revenues.Add(revenue);

                // ================= EXPENSE =================

                decimal expense =
                    _context.CompanyExpenses
                    .Where(x =>
                        x.CreatedDate.Month == i
                        &&
                        x.CreatedDate.Year == currentYear
                    )
                    .Sum(x => (decimal?)x.Amount)
                    ?? 0;

                expenses.Add(expense);

                // ================= PROFIT =================

                decimal profit =
                    revenue - expense;

                profits.Add(profit);
            }

            // ================= SERIALIZE =================

            ViewBag.MonthLabels =
                JsonSerializer.Serialize(months);

            ViewBag.RevenueData =
                JsonSerializer.Serialize(revenues);

            ViewBag.ExpenseData =
                JsonSerializer.Serialize(expenses);

            ViewBag.ProfitData =
                JsonSerializer.Serialize(profits);

            // ================= TOP SALES =================

            var topSales =
                _context.SaleOrders
                .GroupBy(x => x.Sale!.Name)
                .Select(g => new
                {
                    Name = g.Key,

                    Revenue =
                        g.Sum(x => x.Price),

                    CarsSold =
                        g.Count()
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            ViewBag.TopSales =
                topSales;

            return View();
        }
    }
}