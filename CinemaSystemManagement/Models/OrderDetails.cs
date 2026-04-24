namespace CinemaSystemManagement.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }
    }
}
