using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Models;
public class ConsultController : Controller
{
    private readonly ApplicationDbContext _context;

    public ConsultController(ApplicationDbContext context)
    {
        _context = context;
    }

//     [HttpPost]
//     public IActionResult Send(Consult model)
//     {
//         model.CreatedAt = DateTime.Now;

//         _context.Consults.Add(model);
//         _context.SaveChanges();

//         return Content("OK DA LUU");
//     }
// }
[HttpPost]
public IActionResult Send(Consult model)
{
    return Content("FORM DA GUI");
}
}