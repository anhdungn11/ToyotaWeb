using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using ToyotaWeb.Services;

namespace ToyotaWeb.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize(Roles = "Admin,Accountant")]
    public class SalaryController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly EmailService _emailService;

        public SalaryController(
            ApplicationDbContext context,
            EmailService emailService
        )
        {
            _context = context;

            _emailService = emailService;
        }

        // ================= DANH SÁCH =================

        public IActionResult Index()
        {
            var salaries =
                _context.EmployeeSalaries
                .Include(x => x.Sale)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();

            return View(salaries);
        }

        // ================= CREATE =================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Sales =
                _context.Sales.ToList();

            return View();
        }

        // ================= API LẤY DỮ LIỆU LƯƠNG =================

        [HttpGet]
        public JsonResult GetSalaryInfo(
            int saleId,
            int month,
            int year
        )
        {
            var orders =
                _context.SaleOrders
                .Where(x =>
                    x.SaleId == saleId
                    &&
                    x.CreatedDate.Month == month
                    &&
                    x.CreatedDate.Year == year
                )
                .ToList();

            decimal revenue =
                orders.Sum(x => x.Price);

            int carsSold =
                orders.Count();

            decimal commission =
                orders.Sum(x => x.SaleCommission);

            decimal bonus = 0;

            if (carsSold >= 3)
            {
                bonus += 5000000;
            }

            if (revenue >= 5000000000)
            {
                bonus += 10000000;
            }

            return Json(new
            {
                revenue,
                carsSold,
                commission,
                bonus
            });
        }

        // ================= CREATE POST =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeSalary model)
        {
            try
            {
                // ================= CHECK SALE =================

                var sale =
                    _context.Sales
                    .FirstOrDefault(x => x.Id == model.SaleId);

                if (sale == null)
                {
                    return Content("Không tìm thấy sale");
                }

                // ================= CHECK TRÙNG =================

                bool existed =
                    _context.EmployeeSalaries.Any(x =>
                        x.SaleId == model.SaleId
                        &&
                        x.Month == model.Month
                        &&
                        x.Year == model.Year
                    );

                if (existed)
                {
                    return Content(
                        "Nhân viên này đã có bảng lương tháng này"
                    );
                }

                // ================= AUTO INFO =================

                model.EmployeeName =
                    sale.Name;

                model.Email =
                    sale.Email;

                model.Phone =
                    sale.Phone;

                model.Branch =
                    "Toyota Bình Dương";

                model.Position =
                    "Nhân viên Sale";

                // ================= ĐƠN HÀNG =================

                var orders =
                    _context.SaleOrders
                    .Where(x =>
                        x.SaleId == model.SaleId
                        &&
                        x.CreatedDate.Month == model.Month
                        &&
                        x.CreatedDate.Year == model.Year
                    )
                    .ToList();

                // ================= DOANH THU =================

                decimal revenue =
                    orders.Sum(x => x.Price);

                model.TotalRevenue =
                    revenue;

                // ================= SỐ XE =================

                model.CarsSold =
                    orders.Count();

                // ================= HOA HỒNG =================

                decimal commission =
                    orders.Sum(x => x.SaleCommission);

                model.Commission =
                    commission;

                // ================= BONUS KPI =================

                decimal bonus = 0;

                if (model.CarsSold >= 3)
                {
                    bonus += 5000000;
                }

                if (revenue >= 5000000000)
                {
                    bonus += 10000000;
                }

                if (revenue >= 10000000000)
                {
                    bonus += 25000000;
                }

                model.Bonus =
                    bonus;

                // ================= INSURANCE =================

                model.Insurance =
                    model.BaseSalary * 0.105m;

                // ================= TOTAL =================

                decimal totalIncome =
                    model.BaseSalary
                    + model.Commission
                    + model.Bonus
                    + (model.Allowance ?? 0);

                // ================= TAX =================

                model.PersonalTax =
                    totalIncome * 0.1m;

                // ================= NET SALARY =================

                model.NetSalary =
                    totalIncome
                    - (model.Insurance ?? 0)
                    - (model.PersonalTax ?? 0);

                // ================= SAVE =================

                _context.EmployeeSalaries.Add(model);

                _context.SaveChanges();

                TempData["success"] =
                    "Tạo bảng lương thành công";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }

        // ================= EDIT =================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var salary =
                _context.EmployeeSalaries
                .FirstOrDefault(x => x.Id == id);

            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeSalary model)
        {
            try
            {
                var salary =
                    _context.EmployeeSalaries
                    .FirstOrDefault(x => x.Id == model.Id);

                if (salary == null)
                {
                    return NotFound();
                }

                salary.BaseSalary =
                    model.BaseSalary;

                salary.Allowance =
                    model.Allowance;

                // ================= TÍNH LẠI =================

                decimal totalIncome =
                    salary.BaseSalary
                    + salary.Commission
                    + salary.Bonus
                    + (salary.Allowance ?? 0);

                salary.Insurance =
                    salary.BaseSalary * 0.105m;

                salary.PersonalTax =
                    totalIncome * 0.1m;

                salary.NetSalary =
                    totalIncome
                    - (salary.Insurance ?? 0)
                    - (salary.PersonalTax ?? 0);

                _context.SaveChanges();

                TempData["success"] =
                    "Cập nhật lương thành công";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }

        // ================= DELETE =================

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var salary =
                _context.EmployeeSalaries
                .FirstOrDefault(x => x.Id == id);

            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var salary =
                _context.EmployeeSalaries
                .FirstOrDefault(x => x.Id == id);

            if (salary == null)
            {
                return NotFound();
            }

            _context.EmployeeSalaries.Remove(salary);

            _context.SaveChanges();

            TempData["success"] =
                "Xóa bảng lương thành công";

            return RedirectToAction("Index");
        }

        // ================= PAY + SEND EMAIL =================

        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var salary =
                _context.EmployeeSalaries
                .FirstOrDefault(x => x.Id == id);

            if (salary == null)
            {
                return NotFound();
            }

            // ================= UPDATE =================

            salary.IsPaid = true;

            salary.PaidDate = DateTime.Now;

            _context.SaveChanges();

            // ================= HTML MAIL =================

            string html = $@"

<div style='
    font-family:Segoe UI;
    background:#0f172a;
    padding:40px;
    color:white;
'>

    <div style='
        max-width:700px;
        margin:auto;
        background:#111827;
        border-radius:20px;
        padding:35px;
        border:1px solid #334155;
    '>

        <h1 style='
            color:#22c55e;
            margin-bottom:10px;
        '>
            💰 PHIẾU LƯƠNG NHÂN VIÊN
        </h1>

        <p style='color:#94a3b8'>
            Toyota Bình Dương
        </p>

        <hr style='border-color:#334155;margin:25px 0'/>

        <h2 style='margin-bottom:20px'>
            👤 THÔNG TIN NHÂN VIÊN
        </h2>

        <table style='width:100%;line-height:35px'>

            <tr>
                <td><b>Mã nhân viên:</b></td>
                <td>SALE_{salary.SaleId}</td>
            </tr>

            <tr>
                <td><b>Họ tên:</b></td>
                <td>{salary.EmployeeName}</td>
            </tr>

            <tr>
                <td><b>Email:</b></td>
                <td>{salary.Email}</td>
            </tr>

            <tr>
                <td><b>Số điện thoại:</b></td>
                <td>{salary.Phone}</td>
            </tr>

            <tr>
                <td><b>Chức vụ:</b></td>
                <td>{salary.Position}</td>
            </tr>

            <tr>
                <td><b>Chi nhánh:</b></td>
                <td>{salary.Branch}</td>
            </tr>

            <tr>
                <td><b>Kỳ lương:</b></td>
                <td>{salary.Month}/{salary.Year}</td>
            </tr>

            <tr>
                <td><b>Ngày thanh toán:</b></td>
                <td>{salary.PaidDate:dd/MM/yyyy HH:mm}</td>
            </tr>

        </table>

        <hr style='border-color:#334155;margin:25px 0'/>

        <h2 style='margin-bottom:20px'>
            📊 CHI TIẾT LƯƠNG
        </h2>

        <table style='width:100%;line-height:40px'>

            <tr>
                <td>Lương cơ bản</td>
                <td style='text-align:right'>
                    {salary.BaseSalary:N0} đ
                </td>
            </tr>

            <tr>
                <td>Bonus KPI</td>
                <td style='text-align:right;color:#38bdf8'>
                    {salary.Bonus:N0} đ
                </td>
            </tr>

            <tr>
                <td>Hoa hồng sale</td>
                <td style='text-align:right;color:#22c55e'>
                    {salary.Commission:N0} đ
                </td>
            </tr>

            <tr>
                <td>Phụ cấp</td>
                <td style='text-align:right'>
                    {(salary.Allowance ?? 0):N0} đ
                </td>
            </tr>

            <tr>
                <td>Bảo hiểm</td>
                <td style='text-align:right;color:#ef4444'>
                    - {(salary.Insurance ?? 0):N0} đ
                </td>
            </tr>

            <tr>
                <td>Thuế TNCN</td>
                <td style='text-align:right;color:#ef4444'>
                    - {(salary.PersonalTax ?? 0):N0} đ
                </td>
            </tr>

        </table>

        <hr style='border-color:#334155;margin:25px 0'/>

        <div style='
            background:#052e16;
            padding:20px;
            border-radius:16px;
            text-align:center;
        '>

            <h1 style='
                color:#22c55e;
                margin:0;
            '>
                💵 THỰC NHẬN:
                {(salary.NetSalary ?? 0):N0} đ
            </h1>

        </div>

        <p style='
            margin-top:30px;
            color:#94a3b8;
            font-size:14px;
            text-align:center;
        '>
            Đây là email tự động từ hệ thống Toyota Bình Dương
        </p>

    </div>

</div>

";

            // ================= SEND MAIL =================

            await _emailService.SendEmailAsync(
                salary.Email,
                "Phiếu lương Toyota Bình Dương",
                html
            );

            TempData["success"] =
                "Đã thanh toán và gửi email";

            return RedirectToAction("Index");
        }

        // ================= EXPORT PDF =================

        [HttpGet]
        public IActionResult ExportPdf(int id)
        {
            var salary =
                _context.EmployeeSalaries
                .FirstOrDefault(x => x.Id == id);

            if (salary == null)
            {
                return NotFound();
            }

            QuestPDF.Settings.License =
                LicenseType.Community;

            var pdf =
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        page.Size(PageSizes.A4);

                        // ================= HEADER =================

                        page.Header()
                            .Column(col =>
                            {
                                col.Item()
                                    .Text("TOYOTA BÌNH DƯƠNG")
                                    .FontSize(28)
                                    .Bold()
                                    .FontColor(Colors.Green.Medium);

                                col.Item()
                                    .Text("PHIẾU LƯƠNG NHÂN VIÊN")
                                    .FontSize(20)
                                    .SemiBold();
                            });

                        // ================= CONTENT =================

                        page.Content()
                            .Column(col =>
                            {
                                col.Spacing(12);

                                col.Item().Text(
                                    $"Mã nhân viên: SALE_{salary.SaleId}"
                                );

                                col.Item().Text(
                                    $"Họ tên: {salary.EmployeeName}"
                                );

                                col.Item().Text(
                                    $"Email: {salary.Email}"
                                );

                                col.Item().Text(
                                    $"Số điện thoại: {salary.Phone}"
                                );

                                col.Item().Text(
                                    $"Chức vụ: {salary.Position}"
                                );

                                col.Item().Text(
                                    $"Chi nhánh: {salary.Branch}"
                                );

                                col.Item().Text(
                                    $"Kỳ lương: {salary.Month}/{salary.Year}"
                                );

                                col.Item().Text(
                                    $"Ngày xuất phiếu: {DateTime.Now:dd/MM/yyyy HH:mm}"
                                );

                                col.Item().LineHorizontal(1);

                                col.Item().Text(
                                    $"Lương cơ bản: {salary.BaseSalary:N0} đ"
                                );

                                col.Item().Text(
                                    $"Bonus KPI: {salary.Bonus:N0} đ"
                                );

                                col.Item().Text(
                                    $"Hoa hồng: {salary.Commission:N0} đ"
                                );

                                col.Item().Text(
                                    $"Phụ cấp: {(salary.Allowance ?? 0):N0} đ"
                                );

                                col.Item().Text(
                                    $"Bảo hiểm: {(salary.Insurance ?? 0):N0} đ"
                                );

                                col.Item().Text(
                                    $"Thuế TNCN: {(salary.PersonalTax ?? 0):N0} đ"
                                );

                                col.Item().LineHorizontal(1);

                                col.Item()
                                    .Text(
                                        $"THỰC NHẬN: {(salary.NetSalary ?? 0):N0} đ"
                                    )
                                    .FontSize(24)
                                    .Bold()
                                    .FontColor(Colors.Green.Darken2);
                            });

                        // ================= FOOTER =================

                        page.Footer()
                            .AlignCenter()
                            .Text(
                                "Toyota Enterprise Payroll System"
                            );
                    });
                })
                .GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"BangLuong_{salary.EmployeeName}_{salary.Month}_{salary.Year}.pdf"
            );
        }
    }
}