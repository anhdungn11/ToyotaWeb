using System.ComponentModel.DataAnnotations;

namespace ToyotaWeb.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        // 🔥 Thông tin thêm
        public string? Job { get; set; }
        public decimal? Budget { get; set; }
        

        // 🔥 Trạng thái CRM
        public string Status { get; set; } = "Chưa xử lý";

        // 🔥 Sale phụ trách
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? InterestedCar { get; set; }
    }
}