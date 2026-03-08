using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Models;

public class SaleController : Controller
{
    public IActionResult Index()
    {
        var sales = new List<Sale>()
        {
            new Sale{
                Id=1,
                Name="Nguyễn Anh Dũng",
                Phone="0384508388",
                Image="NV1.jpg",
                Description="Chuyên tư vấn dòng xe SUV và Sedan"
            },

            new Sale{
                Id=2,
                Name="Nguyễn Hữu Vũ",
                Phone="0392040105",
                Image="NV2.jpg",
                Description="Tư vấn xe gia đình và xe hybrid"
            }
        };

        return View(sales);
    }

    public IActionResult Details(int id)
    {
        var sales = new List<Sale>()
        {
            new Sale{
                Id=1,
                Name="Nguyễn Anh Dũng",
                Phone="0384508388",
                Image="NV1.jpg",
                Description="Chuyên tư vấn dòng xe SUV và Sedan"
            },

            new Sale{
                Id=2,
                Name="Nguyễn Hữu Vũ",
                Phone="0392040105",
                Image="NV2.jpg",
                Description="Tư vấn xe gia đình và xe hybrid"
            }
        };

        var sale = sales.FirstOrDefault(x => x.Id == id);

        return View(sale);
    }
}