namespace CinemaSystemManagement.Models
{
    public class MovieActor
    {
        public int MovieId { get; set; }
        public Products Movie { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; } = new();
    }
}
