using PollingServer.Models.Poll.Answer;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PollingServer.Models.Poll.Question
{
    public class SelectQuestion : BaseQuestion
    {
        [JsonIgnore]
        private static readonly Type answerType = typeof(SelectAnswer);

        public string? DefaultValue { get; set; }
        public List<string>? Options { get; set; }

        [NotMapped, JsonIgnore]
        public override Type AnswerType
        {
            get { return answerType; }
        }
    }
}
