using System.ComponentModel.DataAnnotations.Schema;

namespace Backlog
{
    [Table("stories")]
    public class StoryRecord
    {
        [Column("id")] public long Id { get; private set; }
        [Column("project_id")] public long ProjectId { get; private set; }
        [Column("name")] public string Name { get; private set; }

        private StoryRecord()
        {
        }

        public StoryRecord(long projectId, string name) : this(default(long), projectId, name)
        {
        }

        public StoryRecord(long id, long projectId, string name)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
        }
    }
}