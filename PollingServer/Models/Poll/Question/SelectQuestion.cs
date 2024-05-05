using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll.Question
{
    public class SelectQuestion : AbstractQuestion
    {
        public string? DefaultValue { get; set; }
        public ICollection<string>? Options { get; set; }
    }
}
