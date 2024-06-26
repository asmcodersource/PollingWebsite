using Microsoft.EntityFrameworkCore;
using PollingServer.Models.Poll.Question;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PollingServer.Models.Poll.Answer
{
    [JsonDerivedType(typeof(SelectAnswer))]
    [JsonDerivedType(typeof(TextFieldAnswer))]
    public abstract class BaseAnswer
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? FieldName { get; set; }
        public string? Description { get; set; }
        public string? Discriminator { get; set; } = null!;

        public abstract List<ValidationResult> ValidateByQuestion(BaseQuestion baseQuestion);

        public List<ValidationResult> ValidateObjectByModel()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            Validator.TryValidateObject(this, context, results, true);
            return results;
        }
       
        public static BaseAnswer ParseJsonByExplicitType(string json, Type type)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var question = JsonSerializer.Deserialize(json, type, options) as BaseAnswer;
            if (question is null)
                throw new JsonException("Invalid object deserrialization");
            return question;
        }
    }
}
