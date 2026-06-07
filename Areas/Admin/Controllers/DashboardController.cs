using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using Microsoft.AspNetCore.Authorization;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.SaleOrders.ToList();

            // =========================
            // DOANH THU
            // =========================
            decimal revenue = orders.Sum(x => x.Price);

            // =========================
            // CÔNG NỢ
            // =========================
            decimal debt = orders.Sum(x => x.Debt);

            // =========================
            // THUẾ DOANH NGHIỆP
            // =========================
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

            decimal tax = revenue * taxRate;

            // =========================
            // HOA HỒNG SALE
            // =========================
            decimal totalCommission = 0;

            foreach (var order in orders)
            {
                if (order.Price < 1000000000)
                {
                    totalCommission += 5000000;
                }
                else if (order.Price <= 3500000000)
                {
                    totalCommission += 12000000;
                }
                else
                {
                    totalCommission += 20000000;
                }
            }

            // =========================
            // CHI PHÍ SHOWROOM
            // =========================
            decimal showroomCost = 200000000;

            // =========================
            // LỢI NHUẬN RÒNG
            // =========================
            decimal profit =
                revenue
                - tax
                - totalCommission
                - showroomCost;

            // =========================
            // VIEWBAG
            // =========================
            ViewBag.Revenue = revenue;

            ViewBag.Debt = debt;

            ViewBag.TotalOrders = orders.Count;

            ViewBag.Tax = tax;

            ViewBag.TaxRate = taxRate * 100;

            ViewBag.Commission = totalCommission;

            ViewBag.ShowroomCost = showroomCost;

            ViewBag.Profit = profit;

            return View();
        }
    }
}