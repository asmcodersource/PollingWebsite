using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll.Question
{
    public class TextFieldQuestion : BaseQuestion
    {
        [JsonIgnore]
        private static readonly Type answerType = typeof(TextFieldAnswer);

        [Required]
        public string FieldPlaceholder { get; set; } = string.Empty;

        [NotMapped, JsonIgnore]
        public override Type AnswerType
        {
            get { return answerType; }
        }
    }

}
