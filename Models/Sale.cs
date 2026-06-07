using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToyotaWeb.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
    }
}