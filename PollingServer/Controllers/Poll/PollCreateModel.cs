using PollingServer.Models.Poll;

namespace PollingServer.Controllers.Poll
{
    public class PollCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PollingType Type { get; set; }
        public PollingAccess Access { get; set; }
    }
}
