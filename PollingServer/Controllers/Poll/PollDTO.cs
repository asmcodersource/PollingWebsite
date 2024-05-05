using PollingServer.Models.Poll.Answer;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll;
using PollingServer.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PollingServer.Controllers.Poll
{
    public class PollDTO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(6), MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int OwnerId { get; set; }

        public PollingType Type { get; set; }
        public PollingAccess Access { get; set; }

        public PollDTO(Models.Poll.Poll poll)
        {
            Id = poll.Id;
            Name = poll.Name;
            Description = poll.Description;
            Access = poll.Access;
            Type = poll.Type;
            OwnerId = poll.OwnerId;
        }
    }
}
