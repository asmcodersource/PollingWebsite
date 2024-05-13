using System.ComponentModel.DataAnnotations;

namespace PollingServer.Models.Poll.Answer
{
    public class TextFieldAnswer : BaseAnswer
    {
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
