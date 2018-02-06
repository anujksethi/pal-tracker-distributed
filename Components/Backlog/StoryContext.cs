using Microsoft.EntityFrameworkCore;

namespace Backlog
{
    public class StoryContext : DbContext
    {
        public StoryContext(DbContextOptions<StoryContext> options) : base(options)
        {
        }

        public DbSet<StoryRecord> StoryRecords { get; set; }
    }
}