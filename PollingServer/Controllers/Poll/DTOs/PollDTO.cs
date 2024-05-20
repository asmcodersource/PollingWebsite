using PollingServer.Models.Poll.Answer;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll;
using PollingServer.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.Image;

namespace PollingServer.Controllers.Poll.DTOs
{
    public class PollDTO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(6), MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        public Image? Image { get; set; } = null;

        public DateTime CreatedAt { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public PollingType Type { get; set; }
        public PollingAccess Access { get; set; }

        public PollDTO(Models.Poll.Poll poll)
        {
            Id = poll.Id;
            Title = poll.Title;
            Description = poll.Description;
            Access = poll.Access;
            Type = poll.Type;
            OwnerId = poll.OwnerId;
            Image = poll.Image;
            CreatedAt = poll.CreatedAt;
        }
    }
}
