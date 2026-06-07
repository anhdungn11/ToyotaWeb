using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class SaleOrder
    {
        public int Id { get; set; }

        // =========================
        // THÔNG TIN KHÁCH
        // =========================

        [Required]
        public string CustomerName { get; set; } = "";

        [Required]
        public string CarName { get; set; } = "";

        public decimal? Deposit { get; set; }

        public string? Note { get; set; }
        // =========================
        // HỢP ĐỒNG
        // =========================

        public string? ContractCode { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerAddress { get; set; }

        public string? CitizenId { get; set; }

        public string? CarColor { get; set; }

        public string? CarVersion { get; set; }

        public string? ChassisNumber { get; set; }

        public string? EngineNumber { get; set; }

        public string? PaymentMethod { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public string? Gift { get; set; }

        public string? PenaltyClause { get; set; }

        public string? ContractStatus { get; set; }

        public string? SaleSignature { get; set; }

        // =========================
        // TIỀN
        // =========================

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Paid { get; set; }

        [NotMapped]
        public decimal Debt => Price - Paid;

        // =========================
        // SHOWROOM
        // =========================

        public string Showroom { get; set; } = "";

        // =========================
        // NGÀY TẠO
        // =========================

        public DateTime CreatedDate { get; set; }

        // =========================
        // NHÂN VIÊN SALE
        // =========================

        public int? SaleId { get; set; }

        public Sale? Sale { get; set; }

        // =========================
        // TRẠNG THÁI ĐƠN
        // =========================

        public string Status { get; set; } = "Pending";

        // =========================
        // HOA HỒNG SALE
        // =========================

        [NotMapped]
        public decimal SaleCommission
        {
            get
            {
                if (Price < 1000000000)
                {
                    return 5000000;
                }

                if (Price >= 1000000000 && Price < 3500000000)
                {
                    return 12000000;
                }

                return 20000000;
            }
        }

        // =========================
        // THUẾ DOANH NGHIỆP
        // =========================

        [NotMapped]
        public decimal TaxAmount
        {
            get
            {
                decimal taxRate = 0;

                if (Price <= 3000000000)
                {
                    taxRate = 0.15m;
                }
                else if (Price > 3000000000 && Price <= 50000000000)
                {
                    taxRate = 0.17m;
                }
                else
                {
                    taxRate = 0.20m;
                }

                return Price * taxRate;
            }
        }

        // =========================
        // LỢI NHUẬN THỰC
        // =========================

        [NotMapped]
        public decimal NetProfit
        {
            get
            {
                return Price - TaxAmount - SaleCommission;
            }
        }
    }
}