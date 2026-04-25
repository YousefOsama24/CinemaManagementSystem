using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace CinemaSystemManagement.ViewModels
{
    public class ApplicationUserVM
    {
        public string Id { get; set; }

        public IFormFile? Image { get; set; }
        public string? ProfileImage { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
