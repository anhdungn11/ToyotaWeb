using System.ComponentModel.DataAnnotations;

namespace ToyotaWeb.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required]
        public string CarName { get; set; }

        public string Color { get; set; }

        public string VinNumber { get; set; }

        public int Quantity { get; set; }

        public decimal ImportPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string Branch { get; set; }

        public string Status { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;
    }
}