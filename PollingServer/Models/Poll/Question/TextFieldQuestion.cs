using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll.Question
{
    public class TextFieldQuestion : AbstractQuestion
    {
        [Required]
        public string FieldPlaceholder { get; set; } = string.Empty;
    }

}
