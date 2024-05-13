using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace PollingServer.Models.Image
{
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaybeNull]
        public string? Name { get; set; }
        
        [Required, NotNull]
        public string ContentType { get; set; }

        [Required, NotNull]
        public byte[] Bytes { get; set; }
    }
}
