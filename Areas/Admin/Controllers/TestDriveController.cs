using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
namespace ToyotaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TestDriveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public TestDriveController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TestDrives.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var item = await _context.TestDrives.FindAsync(id);
            if (item == null) return NotFound();

            if (item.IsProcessed)
                return BadRequest("Đã xử lý rồi");

            item.IsProcessed = true;
            item.Status = "Approved";

            string code;
            do
            {
                code = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            }
            while (await _context.TestDrives.AnyAsync(x => x.ConfirmCode == code));

            item.ConfirmCode = code;

            var existedCustomer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Phone == item.Phone);

            if (existedCustomer == null)
            {
                _context.Customers.Add(new Customer
                {
                    FullName = item.FullName,
                    Phone = item.Phone,
                    Email = item.Email,
                    Address = "Chưa cập nhật",
                    Status = "Mới",
                    CreatedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            string mapLink = item.Showroom switch
            {
                "Toyota Bình Dương" => "https://www.google.com/maps?q=10.9804,106.6519",
                "Toyota Thủ Đức" => "https://www.google.com/maps?q=10.8491,106.7720",
                "Toyota Biên Hòa" => "https://www.google.com/maps?q=10.9447,106.8243",
                _ => "https://maps.google.com"
            };

            string subject = "Xác nhận đăng ký lái thử Toyota";

            string body = $@"
<div style='background:#f5f5f5;padding:20px;font-family:Arial,sans-serif'>
    <div style='max-width:600px;margin:auto;background:#fff;border-radius:12px;
                overflow:hidden;box-shadow:0 10px 30px rgba(0,0,0,0.1)'>

        <div style='background:#e60000;color:white;text-align:center;
                    padding:25px;font-size:26px;font-weight:bold'>
            TOYOTA VIỆT NAM
        </div>

        <div style='padding:25px;color:#333;font-size:15px;line-height:1.6'><h2>Xin chào {item.FullName},</h2>

            <p>Yêu cầu đăng ký của bạn đã được 
            <b style='color:green'>xác nhận</b></p>

            <hr style='margin:20px 0'/>

            <p><b>Xe:</b> {item.CarName}</p>
            <p><b>Ngày:</b> {item.TestDate:dd/MM/yyyy}</p>
            <p><b>Giờ:</b> {item.TimeSlot}</p>
            <p><b>Showroom:</b> {item.Showroom}</p>

            <hr style='margin:20px 0'/>

            <h2 style='text-align:center'>MÃ XÁC NHẬN</h2>

            <div style='text-align:center;
                        font-size:32px;
                        font-weight:bold;
                        color:#e60000;
                        letter-spacing:6px;
                        margin:15px 0'>
                {item.ConfirmCode}
            </div>

            <p style='text-align:center'>
                Vui lòng đưa mã này cho nhân viên khi đến showroom<br/>
                Không chia sẻ mã cho người khác
            </p>

            <div style='text-align:center;margin-top:20px'>
                <a href='{mapLink}'
                   style='background:#e60000;
                          color:white;
                          padding:12px 20px;
                          text-decoration:none;
                          border-radius:6px;
                          font-weight:bold;
                          display:inline-block'>
                    Xem đường đi showroom
                </a>
            </div>

            <hr style='margin:20px 0'/>

            <p style='text-align:center;color:#888'>
                Toyota Việt Nam cảm ơn bạn!
            </p>
        </div>
    </div>
</div>";

            if (!string.IsNullOrEmpty(item.Email))
            {
                await _emailSender.SendEmailAsync(item.Email, subject, body);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Reject(int id)
{
    var item = await _context.TestDrives.FindAsync(id);

    if (item == null)
        return NotFound();

    if (item.IsProcessed)
        return BadRequest("Đã xử lý rồi");

    item.IsProcessed = true;
    item.Status = "Rejected";

    await _context.SaveChangesAsync();

    string subject = "Thông báo đăng ký lái thử Toyota";

    string body = $@"
<div style='background:#f5f5f5;padding:20px;font-family:Arial,sans-serif'>
    <div style='max-width:600px;margin:auto;background:#fff;border-radius:12px;
                overflow:hidden;box-shadow:0 10px 30px rgba(0,0,0,0.1)'>

        <div style='background:#e60000;color:white;text-align:center;
                    padding:25px;font-size:26px;font-weight:bold'>
            TOYOTA VIỆT NAM
        </div>

        <div style='padding:25px;color:#333'>

            <h2>Xin chào {item.FullName},</h2>

            <p>Cảm ơn bạn đã đăng ký lái thử <b>{item.CarName}</b>.</p>

            <p>Rất tiếc, hiện tại chúng tôi chưa thể sắp xếp lịch theo yêu cầu của bạn.</p>

            <p>Chúng tôi sẽ liên hệ lại để hỗ trợ bạn trong thời gian sớm nhất.</p>

            <hr/>

            <p style='text-align:center;color:#888'>
                Toyota Việt Nam xin lỗi vì sự bất tiện này.
            </p>
        </div>
    </div>
</div>";

    if (!string.IsNullOrEmpty(item.Email))
    {
        await _emailSender.SendEmailAsync(item.Email, subject, body);
    }

    return RedirectToAction(nameof(Index)); 
}
    }
}