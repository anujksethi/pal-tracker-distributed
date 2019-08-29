using Microsoft.EntityFrameworkCore;

namespace Users
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        
        public DbSet<UserRecord> UserRecords { get; set; }
    }
}