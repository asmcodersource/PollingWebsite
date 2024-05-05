using Microsoft.EntityFrameworkCore;
using PollingServer.Models.User;
using PollingServer.Models.Poll;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;

namespace PollingServer.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Poll.Poll> Polls { get; protected set; }
        public DbSet<SelectQuestion> SelectQuestions { get; protected set; }
        public DbSet<TextFieldQuestion> TextFieldQuestions { get; protected set; }
        public DbSet<SelectAnswer> SelectResponses { get; protected set; }
        public DbSet<TextFieldAnswer> TextFieldAnswers { get; protected set; }
        public DbSet<User.User> Users { get; protected set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {
            Database.EnsureCreated();
        }
    }
}
