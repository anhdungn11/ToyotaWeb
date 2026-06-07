using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Models;
using ToyotaWeb.Data;

namespace ToyotaWeb.Controllers
{
    public class SOSController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SOSController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Create(SOSRequest model, IFormFile file)
{
    if (file != null)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        model.ImagePath = "/images/" + fileName;
    }

    model.Status = "Pending";

    if (string.IsNullOrEmpty(model.Description))
        model.Description = "Không có mô tả";

    _context.SOSRequests.Add(model);
    await _context.SaveChangesAsync();

    return RedirectToAction("Create");
}

    }
}