namespace CinemaSystemManagement.Models
{
    public class Order
    {
        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
