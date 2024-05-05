using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models.Poll.Question
{
    public class SelectFieldQuestion : BaseQuestion
    {
        public ICollection<string>? Options { get; set; }
        public ICollection<SelectFieldResponse>? Responses { get; set;}
    }
}
