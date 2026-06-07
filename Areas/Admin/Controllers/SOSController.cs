using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SOSController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SOSController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.SOSRequests
                .OrderByDescending(x => x.Id)
                .ToList();

            return View(list);
        }
    }
}