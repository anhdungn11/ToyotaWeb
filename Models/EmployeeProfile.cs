using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class EmployeeProfile
    {
        public int Id { get; set; }

        public string? EmployeeCode { get; set; }

        public string? FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? CitizenId { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Position { get; set; }

        public string? Department { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        public DateTime? JoinDate { get; set; }

        public string? Status { get; set; }

        public string? Education { get; set; } = "Đang làm việc";

        public int ExperienceYears { get; set; }

        public string? EmergencyContact { get; set; }

        public string? Avatar { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;
    }
}