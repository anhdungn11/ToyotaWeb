using System.ComponentModel.DataAnnotations;

namespace ToyotaWeb.Models
{
    public class CarVariant
    {
        [Key]
        public int VariantId { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; } = null!;

        public string VariantName { get; set; } = null!;
        public string Engine { get; set; } = null!;
        public string Transmission { get; set; } = null!;
        public string DriveType { get; set; } = null!;
        public decimal Price { get; set; }
        public string Slug { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<CarImage> Images { get; set; } = new List<CarImage>();

    }
}