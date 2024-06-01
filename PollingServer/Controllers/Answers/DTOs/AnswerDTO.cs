using PollingServer.Models.Poll.Answer;
using PollingServer.Models.Poll;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PollingServer.Controllers.Answers.DTOs
{
    public class AnswerDTO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string UserNickname { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;

        [Required, NotNull]
        public ICollection<BaseAnswer>? Answers { get; set; }

        public AnswerDTO(PollAnswers pollAnswers)
        {
            Id = pollAnswers.Id;
            UserId = pollAnswers.UserId;
            AnswerTime = pollAnswers.AnswerTime;
            Answers = pollAnswers.BaseAnswers;
            UserNickname = pollAnswers.User.Nickname;
        }
    }
}
