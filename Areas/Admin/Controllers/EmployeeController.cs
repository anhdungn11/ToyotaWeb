using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;

namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // DANH SÁCH NHÂN VIÊN
        // =========================

        public IActionResult Index()
        {
            var employees =
                _context.EmployeeProfiles
                .OrderByDescending(x => x.Id)
                .ToList();

            return View(employees);
        }

        // =========================
        // CHI TIẾT NHÂN VIÊN
        // =========================

        public IActionResult Details(int id)
        {
            var employee =
                _context.EmployeeProfiles
                .FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // =========================
        // GET CREATE
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // POST CREATE
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(
            EmployeeProfile employee,
            IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(ImageFile.FileName);

                string folder =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fullPath =
                    Path.Combine(folder, fileName);

                using (var stream =
                    new FileStream(fullPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                employee.Avatar =
                    "/uploads/" + fileName;
            }

            employee.EmployeeCode =
                "EMP-" +
                DateTime.Now.ToString("ddMMyyyyHHmmss");

            _context.EmployeeProfiles.Add(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // =========================
        // GET EDIT
        // =========================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee =
                _context.EmployeeProfiles
                .FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // =========================
        // POST EDIT
        // =========================

        [HttpPost]
        public async Task<IActionResult> Edit(
            EmployeeProfile employee,
            IFormFile? ImageFile)
        {
            var oldEmployee =
                _context.EmployeeProfiles
                .FirstOrDefault(x => x.Id == employee.Id);

            if (oldEmployee == null)
            {
                return NotFound();
            }

            oldEmployee.FullName = employee.FullName;
            oldEmployee.Phone = employee.Phone;
            oldEmployee.Email = employee.Email;
            oldEmployee.Address = employee.Address;

            oldEmployee.Department = employee.Department;
            oldEmployee.Position = employee.Position;

            oldEmployee.BaseSalary = employee.BaseSalary;

            oldEmployee.JoinDate = employee.JoinDate;

            oldEmployee.Status = employee.Status;

            if (ImageFile != null)
            {
                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(ImageFile.FileName);

                string folder =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/uploads");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fullPath =
                    Path.Combine(folder, fileName);

                using (var stream =
                    new FileStream(fullPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                oldEmployee.Avatar =
                    "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // ===============================
        // XÓA NHÂN VIÊN
        // ===============================

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.EmployeeProfiles.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.EmployeeProfiles.FindAsync(id);

            if (employee != null)
            {
                _context.EmployeeProfiles.Remove(employee);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}