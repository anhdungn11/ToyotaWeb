using System.ComponentModel.DataAnnotations;

namespace ToyotaWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        public string Password { get; set; }
    }
}