using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll.Question
{
    public class TextFieldQuestion : BaseQuestion
    {
        [Required]
        public string FieldPlaceholder { get; set; } = string.Empty;
    }

}
