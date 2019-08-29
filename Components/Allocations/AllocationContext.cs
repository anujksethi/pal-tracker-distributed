using Microsoft.EntityFrameworkCore;

namespace Allocations
{
    public class AllocationContext : DbContext
    {
        public AllocationContext(DbContextOptions<AllocationContext> options) : base(options)
        {
        }

        public DbSet<AllocationRecord> AllocationRecords { get; set; }
    }
}