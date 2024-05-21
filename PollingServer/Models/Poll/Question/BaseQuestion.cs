using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PollingServer.Models.Poll.Question
{

    [JsonDerivedType(typeof(SelectQuestion))]
    [JsonDerivedType(typeof(TextFieldQuestion))]
    public class BaseQuestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string FieldName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public int OrderRate { get; set; } = int.MaxValue;

        [Required]
        public string Discriminator { get; set; }


        public static BaseQuestion ParseJsonByDiscriminator(string json, string discriminator)
        {
            var type = BaseQuestion.CreateByDiscriminator(discriminator).GetType();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var question = JsonSerializer.Deserialize(json, type, options) as BaseQuestion;
            if (question is null)
                throw new JsonException("Invalid object deserrialization");
            return question;
        }

        public static BaseQuestion CreateByDiscriminator(string discriminator)
            => discriminator switch
            {
                nameof(TextFieldQuestion) => new TextFieldQuestion(),
                nameof(SelectQuestion) => new SelectQuestion(),
                _ => throw new ArgumentException("Invalid discriminator value", nameof(discriminator))
            };
    }
}
