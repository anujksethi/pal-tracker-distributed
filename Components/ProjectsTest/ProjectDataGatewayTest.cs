using System.Linq;
using Microsoft.EntityFrameworkCore;
using Projects;
using TestSupport;
using Xunit;

namespace ProjectsTest
{
    [Collection("Projects")]
    public class ProjectDataGatewayTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.RegistrationConnectionString);

        private static readonly DbContextOptions<ProjectContext> DbContextOptions =
            new DbContextOptionsBuilder<ProjectContext>().UseMySql(TestDatabaseSupport.RegistrationConnectionString)
                .Options;

        public ProjectDataGatewayTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestCreate()
        {
            Support.ExecSql(@"
insert into users (id, name) values (12, 'Jack');
insert into accounts (id, owner_id, name) values (1, 12, 'anAccount');");

            var gateway = new ProjectDataGateway(new ProjectContext(DbContextOptions));
            gateway.Create(1, "aProject");

            // todo...
            var projects = Support.QuerySql("select name from projects where account_id = 1");

            Assert.Equal("aProject", projects[0]["name"]);
        }

        [Fact]
        public void TestFind()
        {
            Support.ExecSql(@"
insert into users (id, name) values (12, 'Jack');
insert into accounts (id, owner_id, name) values (1, 12, 'anAccount');
insert into projects (id, account_id, name) values (22, 1, 'aProject');");

            var gateway = new ProjectDataGateway(new ProjectContext(DbContextOptions));
            var list = gateway.FindBy(1);

            // todo...
            var actual = list.First();
            Assert.Equal(22, actual.Id);
            Assert.Equal(1, actual.AccountId);
            Assert.Equal("aProject", actual.Name);
            Assert.Equal(true, actual.Active);
        }

        [Fact]
        public void TestFindObject()
        {
            Support.ExecSql(@"
insert into users (id, name) values (12, 'Jack');
insert into accounts (id, owner_id, name) values (1, 12, 'anAccount');
insert into projects (id, account_id, name, active) values (22, 1, 'aProject', true);");

            var gateway = new ProjectDataGateway(new ProjectContext(DbContextOptions));
            var actual = gateway.FindObject(22);

            // todo...    
            Assert.Equal(22, actual.Id);
            Assert.Equal(1, actual.AccountId);
            Assert.Equal("aProject", actual.Name);
            Assert.Equal(true, actual.Active);

            Assert.Null(gateway.FindObject(23));
        }
    }
}