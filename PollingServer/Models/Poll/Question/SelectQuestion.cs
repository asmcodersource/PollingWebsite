using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll.Question
{
    public class SelectQuestion : BaseQuestion
    {
        public string? DefaultValue { get; set; }
        public List<string>? Options { get; set; }
    }
}
