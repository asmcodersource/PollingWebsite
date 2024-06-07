using PollingServer.Models.Poll;

namespace PollingServer.Controllers.Polls.DTOs
{
    public record UpdatePollDTO(string Title, string Description, PollingType Type);
}
