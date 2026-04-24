namespace CinemaSystemManagement.Models
{
    public class Cart
    {
        public int Id { get; set; }
        /*- CustomerId
- TotalPrice
- Status
- Date*/

        public int CustomerId { get; set; }
        public  int  ProductId { get; set; }
        public Products Products { get; set; }
        public int Count { get; set; } = 1;
        

    }
}
