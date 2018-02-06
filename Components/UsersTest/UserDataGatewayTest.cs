using Microsoft.EntityFrameworkCore;
using TestSupport;
using Users;
using Xunit;

namespace UsersTest
{
    [Collection("Users")]
    public class UsersDataGatewayTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.RegistrationConnectionString);

        private static readonly DbContextOptions<UserContext> DbContextOptions =
            new DbContextOptionsBuilder<UserContext>().UseMySql(TestDatabaseSupport.RegistrationConnectionString)
                .Options;

        public UsersDataGatewayTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestCreate()
        {
            var gateway = new UserDataGateway(new UserContext(DbContextOptions));
            gateway.Create("aUser");

            var names = Support.QuerySql("select name from users");

            Assert.Equal("aUser", names[0]["name"]);
        }

        [Fact]
        public void TestFindBy()
        {
            Support.ExecSql(
                @"insert into users (id, name) values (42346, 'aName'), (42347, 'anotherName'), (42348, 'andAnotherName');");

            var gateway = new UserDataGateway(new UserContext(DbContextOptions));

            var actual = gateway.FindObjectBy(42347);

            Assert.Equal(42347, actual.Id);
            Assert.Equal("anotherName", actual.Name);

            Assert.Null(gateway.FindObjectBy(42));
        }
    }
}