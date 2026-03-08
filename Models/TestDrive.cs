using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    [Table("DangKyLaiThus")]
public class TestDrive
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Họ và tên")]
    [Column("HoTen")]
    public string? FullName { get; set; }

    [Required]
    [Display(Name = "Số điện thoại")]
    [Column("SoDienThoai")]
    public string? Phone { get; set; }

    [Required]
    [EmailAddress]
    [Column("Email")]
    public string? Email { get; set; }

    [Display(Name = "Mẫu xe")]
    [Column("DongXe")]
    public string? CarName { get; set; }

    [Display(Name = "Ngày đăng ký")]
    [Column("NgayDangKy")]
    public DateTime? RegisterDate { get; set; }

    [Column("GhiChu")]
    public string? Note { get; set; }
    public bool IsProcessed { get; set; } = false;
    [Required(ErrorMessage = "Vui lòng chọn ngày lái thử")]
[DataType(DataType.Date)]
public DateTime? TestDate { get; set; }

[Required(ErrorMessage = "Vui lòng chọn khung giờ")]
public string? TimeSlot { get; set; }
}
}