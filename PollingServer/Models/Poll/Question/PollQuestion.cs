using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PollingServer.Models.Poll.Question
{
    public class PollQuestion
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string FieldName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; } = string.Empty;
        
        [Required]
        public int OrderRate { get; set; } = int.MinValue;

        [Required, NotNull]
        public BaseQuestion? Question { get; set; }
    }
}
