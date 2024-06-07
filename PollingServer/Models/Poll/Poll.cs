using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.User;
using PollingServer.Models.Image;
using PollingServer.Models.Poll.Question;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PollingServer.Models.Poll
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PollingType
    {
        Anyone, // Anyone can access 
        Authorized, // Only authorized can access
        OnlyAllowed, // Only authorized and allowed can access 
        OnlyOwner, // Only owner can access
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PollingAccess
    {
        Public, // Shown on pages
        Private, // Access only by link
    }

    public class Poll
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(6), MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        public Image.Image? Image { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int OwnerId { get; set; }


        [ForeignKey(nameof(OwnerId))]
        public User.User? Owner { get; set; }

        public virtual ICollection<PollAnswers>? Answers { get; set; }
        public virtual ICollection<BaseQuestion>? Questions { get; set; }
        public virtual ICollection<PollAllowedUsers>? AllowedUsers { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PollingType Type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PollingAccess Access { get; set; }
    }
}
