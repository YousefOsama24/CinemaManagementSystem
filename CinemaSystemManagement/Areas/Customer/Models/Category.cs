namespace CinemaSystemManagement.Areas.Customer.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; } = new();

    }
}
