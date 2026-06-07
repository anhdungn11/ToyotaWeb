using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    [Table("DangKyLaiThus")]
    public class TestDrive
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        [Column("HoTen")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [Column("SoDienThoai")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Dòng xe")]
        public string? CarName { get; set; }

        public DateTime? RegisterDate { get; set; }

        public string? Note { get; set; }

        public bool IsProcessed { get; set; } = false;

        [Required(ErrorMessage = "Vui lòng chọn ngày lái thử")]
        public DateTime? TestDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khung giờ")]
        public string? TimeSlot { get; set; }

        // ❌ KHÔNG Required ở đây
        public string? Status { get; set; } = "Pending";
        public string? CCCDImage { get; set; }

        public string? LicenseImage { get; set; }
        public string? Location { get; set; }
        public string? Showroom { get; set; }
        public string? ConfirmCode { get; set; }
        public bool IsCheckedIn { get; set; } = false;
    }
}