using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Accounts
{
    public class AccountDataGateway : IAccountDataGateway
    {
        private readonly AccountContext _context;

        public AccountDataGateway(AccountContext context)
        {
            _context = context;
        }

        public AccountRecord Create(long ownerId, string name)
        {
            var recordToCreate = new AccountRecord(ownerId, name);

            _context.AccountRecords.Add(recordToCreate);
            _context.SaveChanges();

            return recordToCreate;
        }

        public List<AccountRecord> FindBy(long ownerId) => _context.AccountRecords
            .AsNoTracking()
            .Where(a => a.OwnerId == ownerId)
            .ToList();
    }
}