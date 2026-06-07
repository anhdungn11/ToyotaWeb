using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class EmployeeSalary
    {
        public int Id { get; set; }

        // ================= SALE =================

        public int? SaleId { get; set; }

        public Sale? Sale { get; set; }

        public string? EmployeeName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int? CarsSold { get; set; }
        // ================= CHI NHÁNH =================

        public string? Branch { get; set; }

        // ================= CHỨC VỤ =================

        public string? Position { get; set; }

        // ================= LƯƠNG =================

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Allowance { get; set; }

        // ================= BẢO HIỂM =================

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Insurance { get; set; }

        // ================= THUẾ TNCN =================

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PersonalTax { get; set; }

        // ================= DOANH THU SALE =================

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalRevenue { get; set; }

        // ================= LƯƠNG THỰC NHẬN =================

        [Column(TypeName = "decimal(18,2)")]
        public decimal? NetSalary { get; set; }

        // ================= THANH TOÁN =================

        public bool IsPaid { get; set; } = false;

        public DateTime? PaidDate { get; set; }

        // ================= THỜI GIAN =================

        public int Month { get; set; }

        public int Year { get; set; }
    }
}