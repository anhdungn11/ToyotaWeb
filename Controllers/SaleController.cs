using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using System.Linq;

public class SaleController : Controller
{
    private readonly ApplicationDbContext _context;

    public SaleController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var sales = _context.Sales.ToList();

        return View(sales);
    }

    public IActionResult Details(int id)
    {
        var sale = _context.Sales.FirstOrDefault(x => x.Id == id);

        if (sale == null)
            return NotFound();

        return View(sale);
    }
    public IActionResult MyCustomers(int id)
{
    var customers = _context.Contacts
        .Where(x => x.SaleId == id)
        .ToList();

    return View(customers);
}
[HttpPost]
public IActionResult MarkAsCalled(int id)
{
    var contact = _context.Contacts.Find(id);

    contact.IsCalled = true;

    _context.SaveChanges();

    return RedirectToAction("MyCustomers", new { id = contact.SaleId });
}
}