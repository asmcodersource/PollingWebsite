using PollingServer.Models.Poll.Answer;
using PollingServer.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PollingServer.Models.Poll;

namespace PollingServer.Controllers.Answers.DTOs
{
    public class AnswersItemDTO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string UserNickname { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;


        public AnswersItemDTO(PollAnswers pollAnswers)
        {
            Id = pollAnswers.Id;
            UserId = pollAnswers.UserId;
            AnswerTime = pollAnswers.AnswerTime;
            UserNickname = pollAnswers.User.Nickname;
        }
    }
}
