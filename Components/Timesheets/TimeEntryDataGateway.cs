using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Timesheets
{
    public class TimeEntryDataGateway : ITimeEntryDataGateway
    {
        private readonly TimeEntryContext _context;

        public TimeEntryDataGateway(TimeEntryContext context)
        {
            _context = context;
        }

        public TimeEntryRecord Create(long projectId, long userId, DateTime date, int hours)
        {
            var recordToCreate = new TimeEntryRecord(projectId, userId, date, hours);

            _context.TimeEntryRecords.Add(recordToCreate);
            _context.SaveChanges();

            return recordToCreate;
        }

        public List<TimeEntryRecord> FindBy(long userId) => _context.TimeEntryRecords
            .AsNoTracking()
            .Where(te => te.UserId == userId)
            .ToList();
    }
}