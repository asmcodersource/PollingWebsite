namespace PollingServer.Controllers.Poll.DTOs
{
    public class CreateQuestionDTO
    {
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionDiscriminator { get; set; }
    }
}
