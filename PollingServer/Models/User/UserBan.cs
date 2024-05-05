using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PollingServer.Models.User
{
    public class UserBan
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Reason { get; set; }
        public string? Description { get; set; }

        [NotNull, Required]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        [NotNull, Required]
        public DateTime? EndTime { get; set; } = null;
    }
}
