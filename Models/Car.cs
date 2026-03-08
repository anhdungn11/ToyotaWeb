using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        // ===== BẮT BUỘC =====
        [Required(ErrorMessage = "Tên xe là bắt buộc")]
        [StringLength(300)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // ===== KHÔNG BẮT BUỘC =====
        [StringLength(100)]
        public string? Slug { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(100)]
        public string? BodyType { get; set; }

        public int? Seats { get; set; }

        [StringLength(100)]
        public string? FuelType { get; set; }

        [StringLength(100)]
        public string? Origin { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public string? VideoUrl {get;set;}

        public bool IsActive { get; set; } = true;

        // ===== NAVIGATION =====
        public ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();

        public ICollection<CarVariant> CarVariants { get; set; } = new List<CarVariant>();

        // ===== Thumbnail tự động =====
        [NotMapped]
        public string? Thumbnail
        {
            get
            {
                return CarVariants?
                    .SelectMany(v => v.Images ?? new List<CarImage>())
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault();
            }
        }
    }
}