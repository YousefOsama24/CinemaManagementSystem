using CinemaSystemManagement.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystemManagement.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;


        [Range(50, 1000, ErrorMessage = "Price must be between 50 and 1000")]
        [PriceValidation(ErrorMessage = "Price must be at least 50")]
        public decimal Price { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime DateTime { get; set; }

        public string MainImg { get; set; } = string.Empty;

        public List<MovieImage> SubImages { get; set; } 

        public int CategoryId { get; set; }
        public Category Category { get; set; } 

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }= new();

        public List<MovieActor> MovieActors { get; set; } 
    }
}
