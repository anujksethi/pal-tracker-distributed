using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Allocations
{
    public class AllocationDataGateway : IAllocationDataGateway
    {
        private readonly AllocationContext _context;

        public AllocationDataGateway(AllocationContext context)
        {
            _context = context;
        }

        public AllocationRecord Create(long projectId, long userId, DateTime firstDay, DateTime lastDay)
        {
            var recordToCreate = new AllocationRecord(projectId, userId, firstDay, lastDay);

            _context.AllocationRecords.Add(recordToCreate);
            _context.SaveChanges();

            return recordToCreate;
        }

        public List<AllocationRecord> FindBy(long projectId) => _context.AllocationRecords
            .AsNoTracking()
            .Where(a => a.ProjectId == projectId)
            .ToList();
    }
}