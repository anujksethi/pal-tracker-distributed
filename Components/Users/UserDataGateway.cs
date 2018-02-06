using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Users
{
    public class UserDataGateway : IUserDataGateway
    {
        private readonly UserContext _context;

        public UserDataGateway(UserContext context)
        {
            _context = context;
        }

        public UserRecord Create(string name)
        {
            var recordToCreate = new UserRecord(name);

            _context.UserRecords.Add(recordToCreate);
            _context.SaveChanges();

            return recordToCreate;
        }

        public UserRecord FindObjectBy(long id) => _context.UserRecords
            .AsNoTracking()
            .DefaultIfEmpty()
            .Single(u => u.Id == id);
    }
}