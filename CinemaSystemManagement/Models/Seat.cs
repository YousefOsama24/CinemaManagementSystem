namespace CinemaSystemManagement.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public int ShowTimeId { get; set; }
        public ShowTime ShowTime { get; set; }

        public string Row { get; set; }=string.Empty;
        public int Number { get; set; }

        public bool IsBooked { get; set; }
        public int ProductsId { get; internal set; }
        public Products Product { get; set; }

    }
}
