using System.ComponentModel.DataAnnotations.Schema;

namespace Projects
{
    [Table("projects")]
    public class ProjectRecord
    {
        [Column("id")] public long Id { get; private set; }
        [Column("account_id")] public long AccountId { get; private set; }
        [Column("name")] public string Name { get; private set; }
        [Column("active")] public bool Active { get; private set; }

        private ProjectRecord()
        {
        }

        public ProjectRecord(long accountId, string name, bool active) : this(default(long), accountId, name, active)
        {
        }

        public ProjectRecord(long id, long accountId, string name, bool active)
        {
            Id = id;
            AccountId = accountId;
            Name = name;
            Active = active;
        }
    }
}