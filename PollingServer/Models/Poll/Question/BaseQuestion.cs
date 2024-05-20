using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
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
        public int OrderRate { get; set; } = int.MinValue;

        public string Discriminator { get; private set; }
    }
}
