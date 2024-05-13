using System.ComponentModel.DataAnnotations;

namespace PollingServer.Models.Poll.Answer
{
    public class SelectAnswer : BaseAnswer
    {
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
