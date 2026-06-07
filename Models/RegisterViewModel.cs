using System.ComponentModel.DataAnnotations;

namespace ToyotaWeb.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}