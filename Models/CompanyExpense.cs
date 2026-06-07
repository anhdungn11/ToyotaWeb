using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class CompanyExpense
    {
        public int Id { get; set; }

        public string? ExpenseName { get; set; }

        public string? Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // SQL chưa có cột này
        // nên dùng NotMapped tránh crash

        [NotMapped]
        public string? Note { get; set; }

        public DateTime CreatedDate { get; set; }
            = DateTime.Now;
    }
}