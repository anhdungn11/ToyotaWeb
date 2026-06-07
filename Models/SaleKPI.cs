using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class SaleKPI
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public EmployeeProfile? Employee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetRevenue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentRevenue { get; set; }

        public int TargetOrders { get; set; }

        public int CurrentOrders { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal KPIPercent { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;
    }
}