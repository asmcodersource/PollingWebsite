using System.ComponentModel.DataAnnotations;

namespace PollingServer.Models.Poll.Answer
{
    public class TextFieldResponse : AbstractResponse
    {
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
