using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public SelectFieldQuestion? SelectQuestion { get; set; }
        public TextFieldQuestion? TextQuestion { get; set; }
    }
}
