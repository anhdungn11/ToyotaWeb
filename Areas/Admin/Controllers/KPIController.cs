using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // LIST KPI
        // =========================

        public async Task<IActionResult> Index()
        {
            var data =
                await _context.SaleKPIs
                .Include(x => x.Employee)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync();

            return View(data);
        }

        // =========================
        // CREATE GET
        // =========================

        public IActionResult Create()
        {
            ViewBag.Employees =
                _context.EmployeeProfiles.ToList();

            return View();
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(
            SaleKPI model)
        {
            if (model.TargetRevenue > 0)
            {
                model.KPIPercent =
                    (model.CurrentRevenue
                    / model.TargetRevenue) * 100;
            }

            _context.SaleKPIs.Add(model);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}