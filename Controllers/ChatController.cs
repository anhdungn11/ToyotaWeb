using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Services;

namespace ToyotaWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GeminiService _gemini;

        public ChatController(ApplicationDbContext context, GeminiService gemini)
        {
            _context = context;
            _gemini = gemini;
        }

      [HttpPost]
public async Task<IActionResult> Ask(string message)
{
    try
    {
        if (string.IsNullOrEmpty(message))
            return Json(new { reply = "Bạn vui lòng nhập nội dung." });

        string lower = message.ToLower();

        var xe = _context.Cars
            .FirstOrDefault(x => lower.Contains(x.Name.ToLower()));

        if (xe != null)
        {
            return Json(new
            {
                reply = $"Giá xe {xe.Name} là {xe.Price:N0} VNĐ"
            });
        }

        var ai = await _gemini.Ask(message);

        return Json(new { reply = ai });
    }
    catch (Exception ex)
    {
        return Json(new { reply = "Lỗi server: " + ex.Message });
    }
}
    }
}