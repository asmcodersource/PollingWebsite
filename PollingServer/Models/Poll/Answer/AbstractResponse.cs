using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PollingServer.Models.Poll.Answer
{
    public abstract class AbstractResponse
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        public int UserId { get; set; }

        [Required, ForeignKey(nameof(UserId))]
        public User.User? User { get; set; }


        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;
    }
}
