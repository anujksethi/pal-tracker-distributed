using System;
using System.Linq;
using Allocations;
using Microsoft.EntityFrameworkCore;
using TestSupport;
using Xunit;

namespace AllocationsTest
{
    [Collection("Allocations")]
    public class AllocationDataGatewayTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.AllocationsConnectionString);

        private static readonly DbContextOptions<AllocationContext> DbContextOptions =
            new DbContextOptionsBuilder<AllocationContext>().UseMySql(TestDatabaseSupport.AllocationsConnectionString)
                .Options;

        public AllocationDataGatewayTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestCreate()
        {
            var gateway = new AllocationDataGateway(new AllocationContext(DbContextOptions));
            gateway.Create(22, 12, DateTime.Now, DateTime.Now);

            // todo...
            var projectIds = Support.QuerySql("select project_id from allocations");

            Assert.Equal(22L, projectIds[0]["project_id"]);
        }

        [Fact]
        public void TestFind()
        {
            Support.ExecSql(@"insert into allocations 
(id, project_id, user_id, first_day, last_day) values (97336, 22, 12, now(), now());");

            var gateway = new AllocationDataGateway(new AllocationContext(DbContextOptions));
            var list = gateway.FindBy(22);

            // todo...
            var actual = list.First();
            Assert.Equal(97336, actual.Id);
            Assert.Equal(22, actual.ProjectId);
            Assert.Equal(12, actual.UserId);
        }
    }
}