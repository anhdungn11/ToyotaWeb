using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ToyotaWeb.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Slug { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string BodyType { get; set; }
        public string? Thumbnail
        {
            get
            {
                return CarVariants?
                    .SelectMany(v => v.Images)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault();
            }
        }
        public int? Seats { get; set; }

        [StringLength(100)]
        public string FuelType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }


        [StringLength(100)]
        public string Origin { get; set; }
        public ICollection<CarImage> CarImages { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } = "";
        public bool IsActive { get; set; } = true;

        public ICollection<CarVariant> CarVariants { get; set; } = new List<CarVariant>();
    }
}