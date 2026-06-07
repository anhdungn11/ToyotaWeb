using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;

namespace ToyotaWeb.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize(Roles = "Admin,Accountant")]
    public class FinanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // ================= ĐƠN HÀNG =================

            var orders =
                _context.SaleOrders
                .ToList();

            decimal revenue =
                orders.Sum(x => x.Price);

            decimal debt =
                orders.Sum(x => x.Debt);

            // ================= THUẾ =================

            decimal taxRate = 0;

            if (revenue <= 3000000000)
            {
                taxRate = 0.15m;
            }
            else if (revenue <= 50000000000)
            {
                taxRate = 0.17m;
            }
            else
            {
                taxRate = 0.20m;
            }

            decimal tax =
                revenue * taxRate;

            // ================= HOA HỒNG SALE =================

            decimal totalCommission = 0;

            foreach (var item in orders)
            {
                if (item.Price < 1000000000)
                {
                    totalCommission += 5000000;
                }
                else if (item.Price < 3500000000)
                {
                    totalCommission += 12000000;
                }
                else
                {
                    totalCommission += 20000000;
                }
            }

            

            decimal totalExpenses =
                _context.CompanyExpenses
                .Sum(x => (decimal?)x.Amount) ?? 0;

            // ================= LƯƠNG NHÂN VIÊN =================

            var salaries =
                _context.EmployeeSalaries
                .ToList();

            decimal totalSalary =
                salaries.Sum(x =>
                    x.BaseSalary +
                    x.Bonus +
                    x.Commission
                );

            // ================= LỢI NHUẬN RÒNG =================

            decimal netProfit =
                revenue
                - tax
                - totalCommission
                - totalExpenses
                - totalSalary;

            // ================= TOP SALE =================

            var topSales =
                orders
                .GroupBy(x => x.SaleId)
                .Select(g => new
                {
                    SaleId = g.Key,
                    Orders = g.Count(),
                    Revenue = g.Sum(x => x.Price)
                }).OrderByDescending(x => x.Revenue)
                .ToList();

            

            ViewBag.TotalRevenue = revenue;

            ViewBag.TotalDebt = debt;

            ViewBag.TotalTax = tax;

            ViewBag.TaxRate = taxRate * 100;

            ViewBag.TotalCommission = totalCommission;

            ViewBag.TotalExpenses = totalExpenses;

            ViewBag.TotalSalary = totalSalary;

            ViewBag.NetProfit = netProfit;

            ViewBag.TotalOrders = orders.Count;

            ViewBag.Orders = orders;

            ViewBag.TopSales = topSales;

            ViewBag.Expenses =
                _context.CompanyExpenses
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            ViewBag.Salaries =
                salaries
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();

            return View();
        }
    }
}