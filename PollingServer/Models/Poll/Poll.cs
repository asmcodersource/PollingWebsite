using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.User;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll
{
    public enum PollingType
    {
        Anonymous,
        NonAnonymous
    }

    public enum PollingVisibility
    {
        Public,
        Private
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

        public ICollection<PollQuestion>? Questions { get; set; }
        public ICollection<PollAllowedUsers>? UsersEligibility { get; set; }
        public PollingType Type { get; set; }
        public PollingVisibility Visibility { get; set; }
    }
}
