using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class CarImage
    {
        [Key]
        public int ImageId { get; set; }

        // Ảnh thuộc Variant
        public int? VariantId { get; set; }

        [ForeignKey("VariantId")]
        public CarVariant? CarVariant { get; set; }

        // Ảnh thuộc Car (dùng cho Home)
        public int? CarId { get; set; }

        [ForeignKey("CarId")]
        public Car? Car { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}