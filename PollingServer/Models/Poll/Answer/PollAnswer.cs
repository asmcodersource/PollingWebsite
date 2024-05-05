﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PollingServer.Models.Poll.Answer
{
    public class PollAnswers
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, ForeignKey(nameof(UserId))]
        public User.User? User { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; } = DateTime.UtcNow;

        [Required, NotNull]
        public ICollection<AbstractAnswer>? Answers { get; set; }

    }
}