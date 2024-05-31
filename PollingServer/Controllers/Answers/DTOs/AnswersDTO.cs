using PollingServer.Models.Poll.Answer;
using PollingServer.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PollingServer.Models.Poll;

namespace PollingServer.Controllers.Answers.DTOs
{
    public class AnswersDTO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;

        [Required, NotNull]
        public ICollection<BaseAnswer>? Answers { get; set; }

        public AnswersDTO(PollAnswers pollAnswers)
        {
            Id = pollAnswers.Id;
            UserId = pollAnswers.UserId;
            AnswerTime = pollAnswers.AnswerTime;
            Answers = pollAnswers.BaseAnswers;
        }
    }
}
