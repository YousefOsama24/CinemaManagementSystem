namespace CinemaSystemManagement.Models.customer
{
    public class Transaction
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }=new();

        public string MovieName { get; set; } = string.Empty;
        public string MovieImg { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
