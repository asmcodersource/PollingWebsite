using Microsoft.EntityFrameworkCore;
using PollingServer.Models.User;
using PollingServer.Models.Poll;

namespace PollingServer.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Poll.Poll> Polls { get; protected set; }
        public DbSet<User.User> Users { get; protected set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {
            Database.EnsureCreated();
        }
    }
}
