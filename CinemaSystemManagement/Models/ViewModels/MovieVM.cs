using CinemaSystemManagement.Models;

namespace CinemaSystemManagement.Models.ViewModels
{
    public class MovieVM
    {
        public Movie Movie { get; set; } = new();

        public List<Category> Categories { get; set; } = new();
        public List<Actor> Actors { get; set; } = new();
        public List<int> SelectedActors { get; internal set; } = new();
    }
}
