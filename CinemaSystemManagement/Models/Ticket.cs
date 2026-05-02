namespace CinemaSystemManagement.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty; // Order, Success, Tracking

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";
    }
}
