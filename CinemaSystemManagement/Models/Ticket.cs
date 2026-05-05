namespace CinemaSystemManagement.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ShowTimeId { get; set; }
        public ShowTime ShowTime { get; set; }

        public int SeatId { get; set; }
        public Seat Seat { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
