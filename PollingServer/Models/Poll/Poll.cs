using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.User;
using PollingServer.Models.Image;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll
{
    public enum PollingType
    {
        Anyone, // Anyone can access 
        Auhorized, // Only authorized can access
        OnlyAllowed, // Only authorized and allowed can access 
        OnlyOwner, // Only owner can access
    }

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
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        public Image.Image? Image { get; set; }

        [Required]
        public int OwnerId { get; set; }


        [ForeignKey(nameof(OwnerId))]
        public User.User? Owner { get; set; }

        public ICollection<PollAnswers>? Answers { get; set; }
        public ICollection<PollQuestion>? Questions { get; set; }
        public ICollection<PollAllowedUsers>? AllowedUsers { get; set; }
        public PollingType Type { get; set; }
        public PollingAccess Access { get; set; }
    }
}
