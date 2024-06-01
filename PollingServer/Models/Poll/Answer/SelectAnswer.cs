using PollingServer.Models.Poll.Question;
using System.ComponentModel.DataAnnotations;

namespace PollingServer.Models.Poll.Answer
{
    public class SelectAnswer : BaseAnswer
    {
        [Required, MaxLength(1024*4)] // Lets assume that 4Kb its maximum length
        public string Text { get; set; } = string.Empty;

        public override List<ValidationResult> ValidateByQuestion(BaseQuestion baseQuestion)
        {
            if (baseQuestion.GetType() != typeof(SelectQuestion))
                throw new Exception("Invalid question type for answer verifing process");

            var question = (SelectQuestion)baseQuestion;
            var validationResults = new List<ValidationResult>();
            if (question.Options.Contains(Text) is not true)
                validationResults.Add(new ValidationResult($"Select answer for question={baseQuestion.Id} provide non-existed option text={Text}"));
            return validationResults;
        }
    }
}
