using System.ComponentModel.DataAnnotations;

namespace PollingServer.Controllers.Answers.DTOs
{
    public class ValidationErrorResponse
    {
        public ICollection<ValidationResult> ValidationResult { get; set; }
        public int QuestionId { get; set; }

        public ValidationErrorResponse(ICollection<ValidationResult> validationResult, int questionId)
        {
            ValidationResult = validationResult;
            QuestionId = questionId;
        }
    }
}
