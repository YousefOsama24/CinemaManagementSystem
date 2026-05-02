namespace CinemaSystemManagement.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
