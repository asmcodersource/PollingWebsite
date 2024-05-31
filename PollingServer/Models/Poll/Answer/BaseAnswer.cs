using Microsoft.EntityFrameworkCore;
using PollingServer.Models.Poll.Question;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PollingServer.Models.Poll.Answer
{
    public abstract class BaseAnswer
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
