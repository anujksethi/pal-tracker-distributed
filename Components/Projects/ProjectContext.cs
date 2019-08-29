using Microsoft.EntityFrameworkCore;

namespace Projects
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }
        
        public DbSet<ProjectRecord> ProjectRecords { get; set; }
    }
}