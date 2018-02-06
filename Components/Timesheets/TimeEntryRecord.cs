using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timesheets
{
    [Table("time_entries")]
    public class TimeEntryRecord
    {
        [Column("id")] public long Id { get; private set; }
        [Column("project_id")] public long ProjectId { get; private set; }
        [Column("user_id")] public long UserId { get; private set; }
        [Column("date")] public DateTime Date { get; private set; }
        [Column("hours")] public int Hours { get; private set; }

        private TimeEntryRecord()
        {
        }

        public TimeEntryRecord(long projectId, long userId, DateTime date, int hours) :
            this(default(long), projectId, userId, date, hours)
        {
        }

        public TimeEntryRecord(long id, long projectId, long userId, DateTime date, int hours)
        {
            Id = id;
            ProjectId = projectId;
            UserId = userId;
            Date = date;
            Hours = hours;
        }
    }
}