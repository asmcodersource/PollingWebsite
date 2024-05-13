using Microsoft.EntityFrameworkCore;
using PollingServer.Models.User;
using PollingServer.Models.Poll;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;
using System.Diagnostics.Contracts;

namespace PollingServer.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Image.Image> Images { get; protected set; }
        public DbSet<Poll.Poll> Polls { get; protected set; }
        public DbSet<User.User> Users { get; protected set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SelectQuestion>();
            modelBuilder.Entity<TextFieldQuestion>();
            modelBuilder.Entity<SelectAnswer>();
            modelBuilder.Entity<TextFieldAnswer>();
        }

    }
}
