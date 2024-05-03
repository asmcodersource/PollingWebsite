using Microsoft.EntityFrameworkCore;

namespace PollingServer.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; protected set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {
            Database.EnsureCreated();
        }
    }
}
