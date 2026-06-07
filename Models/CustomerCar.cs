namespace ToyotaWeb.Models
{
    public class CustomerCar
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string CarName { get; set; }
        public decimal Price { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}