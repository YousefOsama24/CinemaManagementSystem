namespace CinemaSystemManagement.Models
{
    public class ShowTime
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Products Movie { get; set; }

        public DateTime StartTime { get; set; }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }

        public decimal Price { get; set; }

        public List<Seat> Seats { get; set; } = new();
    }
}
