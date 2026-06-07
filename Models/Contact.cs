using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        public string? Email { get; set; }

        public string? CarName { get; set; }

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsCalled { get; set; } = false;

        public string? CallNote { get; set; }

     
        public int? SaleId { get; set; }

        [ForeignKey("SaleId")]
        public Sale? Sale { get; set; }
    }
}