using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
namespace ToyotaWeb.Areas.Admin.Controllers;
[Area("Garage")]
public class JobsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Complete(int id)
    {
        // cập nhật Done + nhập bill
        return RedirectToAction("Index");
    }
}