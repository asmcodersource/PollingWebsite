using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.User;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll
{
    public enum PollingType
    {
        Anonymous, // Only logged in users can access
        NonAnonymous // Anyone can access 
    }

    public enum PollingVisibility
    {
        Public, // Can be shown somewhere on pages of website
        Private, // Access only by link
        Protected  // Only owner can access
    }

    public class Poll
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(6), MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public User.User? Owner { get; set; }

        public ICollection<PollAnswers>? Answers { get; set; }
        public ICollection<PollQuestion>? Questions { get; set; }
        public ICollection<PollAllowedUsers>? UsersEligibility { get; set; }
        public PollingType Type { get; set; }
        public PollingVisibility Visibility { get; set; }
    }
}
