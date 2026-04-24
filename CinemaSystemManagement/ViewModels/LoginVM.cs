using System.ComponentModel.DataAnnotations;

namespace CinemaSystemManagement.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email or Username is required")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
