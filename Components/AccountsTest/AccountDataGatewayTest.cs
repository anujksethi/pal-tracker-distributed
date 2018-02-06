using Accounts;
using Microsoft.EntityFrameworkCore;
using TestSupport;
using Xunit;

namespace AccountsTest
{
    [Collection("Accounts")]
    public class AccountDataGatewayTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.RegistrationConnectionString);

        private static readonly DbContextOptions<AccountContext> DbContextOptions =
            new DbContextOptionsBuilder<AccountContext>().UseMySql(TestDatabaseSupport.RegistrationConnectionString)
                .Options;

        public AccountDataGatewayTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestCreate()
        {
            Support.ExecSql("insert into users (id, name) values (12, 'Jack');");

            var gateway = new AccountDataGateway(new AccountContext(DbContextOptions));
            gateway.Create(12, "anAccount");

            var names = Support.QuerySql("select name from accounts");

            Assert.Equal("anAccount", names[0]["name"]);
        }

        [Fact]
        public void TestFindBy()
        {
            Support.ExecSql(@"
insert into users (id, name) values (12, 'Jack');
insert into accounts (id, owner_id, name) values (1, 12, 'anAccount'), (2, 12, 'anotherAccount');
");

            var gateway = new AccountDataGateway(new AccountContext(DbContextOptions));

            var actual = gateway.FindBy(12);

            Assert.Equal(1, actual[0].Id);
            Assert.Equal(12, actual[0].OwnerId);
            Assert.Equal("anAccount", actual[0].Name);
            Assert.Equal(2, actual[1].Id);
            Assert.Equal(12, actual[1].OwnerId);
            Assert.Equal("anotherAccount", actual[1].Name);
        }
    }
}