using System.ComponentModel.DataAnnotations;

namespace CinemaSystemManagement.ViewModels
{
    public class ChangePasswordVM
    {
        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
