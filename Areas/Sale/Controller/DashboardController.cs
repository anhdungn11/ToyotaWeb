using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;

namespace ToyotaWeb.Areas.Sale.Controllers
{
    [Area("Sale")]
    [Authorize(Roles = "Sale")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalCustomers = _context.Customers.Count();

            var totalContacts = _context.Contacts.Count();

            var calledContacts =
                _context.Contacts.Count(x => x.IsCalled);

            var pendingContacts =
                _context.Contacts.Count(x => !x.IsCalled);

            var totalOrders =
                _context.SaleOrders.Count();

            var totalTestDrives =
                _context.TestDrives.Count();

            ViewBag.TotalCustomers = totalCustomers;

            ViewBag.TotalContacts = totalContacts;

            ViewBag.CalledContacts = calledContacts;

            ViewBag.PendingContacts = pendingContacts;

            ViewBag.TotalOrders = totalOrders;

            ViewBag.TotalTestDrives = totalTestDrives;

            return View();
        }
    }
}