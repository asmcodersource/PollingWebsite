using PollingServer.Models.Poll.Answer;
using PollingServer.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PollingServer.Controllers.Answers.DTOs
{
    public class PollAnswersDTO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;

        [Required, NotNull]
        public ICollection<BaseAnswer>? Answers { get; set; }

        public PollAnswersDTO(PollAnswers pollAnswers)
        {
            Id = pollAnswers.Id;
            UserId = pollAnswers.UserId;
            AnswerTime = pollAnswers.AnswerTime;
            Answers = pollAnswers.BaseAnswers;
        }
    }
}
