using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Backlog
{
    public class StoryDataGateway : IStoryDataGateway
    {
        private readonly StoryContext _context;

        public StoryDataGateway(StoryContext context)
        {
            _context = context;
        }

        public StoryRecord Create(long projectId, string name)
        {
            var recordToCreate = new StoryRecord(projectId, name);

            _context.StoryRecords.Add(recordToCreate);
            _context.SaveChanges();

            return recordToCreate;
        }

        public List<StoryRecord> FindBy(long projectId) => _context.StoryRecords
            .AsNoTracking()
            .Where(s => s.ProjectId == projectId)
            .ToList();
    }
}