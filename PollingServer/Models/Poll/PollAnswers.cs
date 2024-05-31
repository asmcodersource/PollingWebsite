using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll
{
    public class PollAnswers
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        public int UserId { get; set; }

        [Required, ForeignKey(nameof(UserId))]
        public User.User? User { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;

        [Required, NotNull]
        public ICollection<BaseAnswer>? BaseAnswers { get; set; }

    }
}
