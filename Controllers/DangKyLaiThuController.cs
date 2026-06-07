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
        // GET
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
        // POST
        // =============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestDrive model, IFormFile? cccdFile, IFormFile? licenseFile)
        {
            // 🔥 CHECK FILE
            if (cccdFile == null)
                ModelState.AddModelError("", "Vui lòng tải CCCD");

            if (licenseFile == null)
                ModelState.AddModelError("", "Vui lòng tải bằng lái");

            // 🔥 CHECK NGÀY
            if (model.TestDate < DateTime.Today)
            {
                ModelState.AddModelError("", "Không thể chọn ngày trong quá khứ.");
            }

          
            bool isBooked = _context.TestDrives.Any(x =>
                x.CarName == model.CarName &&
                x.TestDate == model.TestDate &&
                x.TimeSlot == model.TimeSlot &&
                x.IsProcessed == false
            );

            if (isBooked)
            {
                ModelState.AddModelError("", "Khung giờ này đã được đặt.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CarList = _context.Cars.ToList();
                return View(model);
            }

            
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/testdrive");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 🔥 CCCD
            string cccdName = Guid.NewGuid().ToString() + Path.GetExtension(cccdFile!.FileName);
            string cccdPath = Path.Combine(folder, cccdName);

            using (var stream = new FileStream(cccdPath, FileMode.Create))
            {
                await cccdFile.CopyToAsync(stream);
            }

            model.CCCDImage = "/images/testdrive/" + cccdName;

            // 🔥 LICENSE
            string licenseName = Guid.NewGuid().ToString() + Path.GetExtension(licenseFile!.FileName);
            string licensePath = Path.Combine(folder, licenseName);

            using (var stream = new FileStream(licensePath, FileMode.Create))
            {
                await licenseFile.CopyToAsync(stream);
            }

            model.LicenseImage = "/images/testdrive/" + licenseName;

            // =============================
            // SAVE DB
            // =============================
            model.RegisterDate = DateTime.Now;
            model.IsProcessed = false;

            _context.TestDrives.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký lái thử thành công!";

            return RedirectToAction("Create");
        }
    }
}