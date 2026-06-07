namespace ToyotaWeb.Models
{
    public class CustomerInteraction
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;

        public DateTime? NextFollowUpDate { get; set; }
    }
}