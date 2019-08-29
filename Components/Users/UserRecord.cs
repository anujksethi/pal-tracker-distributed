using System.ComponentModel.DataAnnotations.Schema;

namespace Users
{
    [Table("users")]
    public class UserRecord
    {
        [Column("id")] public long Id { get; private set; }
        [Column("name")] public string Name { get; private set; }

        private UserRecord()
        {
        }

        public UserRecord(string name) : this(default(long), name)
        {
        }

        public UserRecord(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}