namespace CinemaSystemManagement.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<Products> Products { get; set; } = new();

    }
}
