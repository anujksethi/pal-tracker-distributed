using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allocations
{
    [Table("allocations")]
    public class AllocationRecord
    {
        [Column("id")] public long Id { get; private set; }
        [Column("project_id")] public long ProjectId { get; private set; }
        [Column("user_id")] public long UserId { get; private set; }
        [Column("first_day")] public DateTime FirstDay { get; private set; }
        [Column("last_day")] public DateTime LastDay { get; private set; }

        private AllocationRecord()
        {
        }

        public AllocationRecord(long projectId, long userId, DateTime firstDay, DateTime lastDay) :
            this(default(long), projectId, userId, firstDay, lastDay)
        {
        }

        public AllocationRecord(long id, long projectId, long userId, DateTime firstDay, DateTime lastDay)
        {
            Id = id;
            ProjectId = projectId;
            UserId = userId;
            FirstDay = firstDay;
            LastDay = lastDay;
        }
    }
}