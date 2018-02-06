using System.Linq;
using Backlog;
using Microsoft.EntityFrameworkCore;
using TestSupport;
using Xunit;

namespace BacklogTest
{
    [Collection("Backlog")]
    public class StoryDataGatewayTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.BacklogConnectionString);

        private static readonly DbContextOptions<StoryContext> DbContextOptions =
            new DbContextOptionsBuilder<StoryContext>().UseMySql(TestDatabaseSupport.BacklogConnectionString).Options;

        public StoryDataGatewayTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestCreate()
        {
            var gateway = new StoryDataGateway(new StoryContext(DbContextOptions));
            gateway.Create(22, "aStory");

            // todo...
            var projectIds = Support.QuerySql("select project_id from stories");

            Assert.Equal(22L, projectIds[0]["project_id"]);
        }

        [Fact]
        public void TestFind()
        {
            Support.ExecSql("insert into stories (id, project_id, name) values (1346, 22, 'aStory');");

            var gateway = new StoryDataGateway(new StoryContext(DbContextOptions));
            var list = gateway.FindBy(22);

            // todo...
            var actual = list.First();
            Assert.Equal(1346, actual.Id);
            Assert.Equal(22, actual.ProjectId);
            Assert.Equal("aStory", actual.Name);
        }
    }
}