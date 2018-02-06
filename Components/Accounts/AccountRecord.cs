using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts
{
    [Table("accounts")]
    public class AccountRecord
    {
        [Column("id")] public long Id { get; private set; }
        [Column("owner_id")] public long OwnerId { get; private set; }
        [Column("name")] public string Name { get; private set; }

        private AccountRecord()
        {
        }

        public AccountRecord(long ownerId, string name) : this(default(long), ownerId, name)
        {
        }

        public AccountRecord(long id, long ownerId, string name)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
        }
    }
}