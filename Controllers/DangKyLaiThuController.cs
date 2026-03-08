using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Models;
using ToyotaWeb.Data;

namespace ToyotaWeb.Controllers
{
    public class DangKyLaiThuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangKyLaiThuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =============================
        // GET: DangKyLaiThu/Create
        // =============================
        public IActionResult Create(string dongXe)
        {
            ViewBag.CarList = _context.Cars.ToList();

            var model = new TestDrive();

            if (!string.IsNullOrEmpty(dongXe))
            {
                model.CarName = dongXe;
            }

            return View(model);
        }

        // =============================
        // POST: DangKyLaiThu/Create
        // =============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestDrive model)
        {
            
            if (model.TestDate < DateTime.Today)
            {
                ModelState.AddModelError("", "Không thể chọn ngày trong quá khứ.");
            }

            // Chống trùng lịch (cùng xe, cùng ngày, cùng giờ)
            bool isBooked = _context.TestDrives.Any(x =>
                x.CarName == model.CarName &&
                x.TestDate == model.TestDate &&
                x.TimeSlot == model.TimeSlot
            );

            if (isBooked)
            {
                ModelState.AddModelError("", "Khung giờ này đã được đặt. Vui lòng chọn giờ khác.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CarList = _context.Cars.ToList();
                return View(model);
            }

            model.RegisterDate = DateTime.Now;
            model.IsProcessed = false;

            _context.TestDrives.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký lái thử thành công!";

            return RedirectToAction("Create");
        }
    }
} 