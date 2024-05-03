using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PollingServer.Models
{
    public class User
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(256), MinLength(1)]
        public string Nickname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(2048), MinLength(6), RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$")]
        public string Password { get; set; }

        [Required, NotNull]
        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
    }
}
