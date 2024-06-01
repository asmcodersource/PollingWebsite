using PollingServer.Models.Poll.Question;
using System.ComponentModel.DataAnnotations;

namespace PollingServer.Models.Poll.Answer
{
    public class TextFieldAnswer : BaseAnswer
    {
        [Required, MaxLength(1024*4)] // Lets assume that 4Kb its maximum length
        public string Text { get; set; } = string.Empty;

        public override List<ValidationResult> ValidateByQuestion(BaseQuestion baseQuestion)
        {
            if (baseQuestion.GetType() != typeof(TextFieldQuestion))
                throw new Exception("Invalid question type for answer verifing process");

            var question = (TextFieldQuestion)baseQuestion;
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
