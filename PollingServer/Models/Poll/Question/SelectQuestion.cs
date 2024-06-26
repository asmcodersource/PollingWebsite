using PollingServer.Models.Poll.Answer;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PollingServer.Models.Poll.Question
{
    public class SelectQuestion : BaseQuestion
    {
        public string? DefaultValue { get; set; }
        public List<string> Options { get; set; } = new List<string>();

        [NotMapped, JsonIgnore]
        public override Type AnswerType { get { return typeof(SelectAnswer); } }
    }
}
