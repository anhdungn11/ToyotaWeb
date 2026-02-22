using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;

public class VariantController : Controller
{
    private readonly ApplicationDbContext _context;

    public VariantController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Details(int id)
{
    var variant = _context.CarVariants
        .Include(v => v.Car)
        .Include(v => v.Images)
        .FirstOrDefault(v => v.VariantId == id);

    return View(variant);
}
}
