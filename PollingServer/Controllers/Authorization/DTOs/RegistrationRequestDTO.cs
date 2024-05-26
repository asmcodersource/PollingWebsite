using System.ComponentModel.DataAnnotations;

namespace PollingServer.Controllers.Authorization.DTOs
{
    public class RegistrationRequestDTO
    {
        [Required(ErrorMessage = "Nickname is required")]
        [MaxLength(256, ErrorMessage = "Nickname must be at most 256 characters long")]
        [MinLength(1, ErrorMessage = "Nickname must be at least 1 character long")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(2048, ErrorMessage = "Password must be at most 2048 characters long")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Password must be minimum eight characters length, have at least one letter and one number")]
        public string Password { get; set; }

        [Required, EmailAddress(ErrorMessage = "Email must be in correct form")]
        public string Email { get; set; }

    }
}
