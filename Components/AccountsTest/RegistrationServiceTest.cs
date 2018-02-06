using Accounts;
using Microsoft.EntityFrameworkCore;
using TestSupport;
using Users;
using Xunit;

namespace AccountsTest
{
    [Collection("Accounts")]
    public class RegistrationServiceTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.RegistrationConnectionString);

        private static readonly DbContextOptions<AccountContext> AccountDbContextOptions =
            new DbContextOptionsBuilder<AccountContext>().UseMySql(TestDatabaseSupport.RegistrationConnectionString)
                .Options;

        private static readonly DbContextOptions<UserContext> UserDbContextOptions =
            new DbContextOptionsBuilder<UserContext>().UseMySql(TestDatabaseSupport.RegistrationConnectionString)
                .Options;

        public RegistrationServiceTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestFindBy()
        {
            var userDataGateway = new UserDataGateway(new UserContext(UserDbContextOptions));
            var accountDataGateway = new AccountDataGateway(new AccountContext(AccountDbContextOptions));
            var service = new RegistrationService(userDataGateway, accountDataGateway);

            var info = service.CreateUserWithAccount("aUser");

            Assert.Equal("aUser", info.Name);
        }
    }
}