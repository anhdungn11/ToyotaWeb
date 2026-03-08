using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Models;
using ToyotaWeb.Data;
using System.Linq;

namespace ToyotaWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
public JsonResult Ask(string message)
{
    if (string.IsNullOrEmpty(message))
        return Json("Bạn vui lòng nhập nội dung.");

    message = message.ToLower();

    // Nếu hỏi giá xe nhưng không nói tên
    if (message.Contains("giá"))
    {
        var danhSachXe = _context.Cars
            .Select(x => x.Name)
            .ToList();

        string ds = string.Join(", ", danhSachXe);

        return Json("Bạn muốn hỏi giá xe nào? Hiện tại Toyota có: " + ds);
    }

    // Nếu có tên xe cụ thể
    var xe = _context.Cars
        .FirstOrDefault(x => message.Contains(x.Name.ToLower()));

    if (xe != null)
    {
        return Json($"Giá xe {xe.Name} hiện tại là {xe.Price:N0} VNĐ");
    }

    return Json("Toyota có thể hỗ trợ bạn về giá xe, khuyến mãi hoặc thông tin sản phẩm.");
}
    }
}