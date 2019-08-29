using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestSupport;
using Timesheets;
using Xunit;

namespace TimesheetsTest
{
    [Collection("Timesheets")]
    public class TimeEntryDataGatewayTest
    {
        private static readonly TestDatabaseSupport Support =
            new TestDatabaseSupport(TestDatabaseSupport.TimesheetsConnectionString);

        private static readonly DbContextOptions<TimeEntryContext> DbContextOptions =
            new DbContextOptionsBuilder<TimeEntryContext>().UseMySql(TestDatabaseSupport.TimesheetsConnectionString)
                .Options;

        public TimeEntryDataGatewayTest()
        {
            Support.TruncateAllTables();
        }

        [Fact]
        public void TestCreate()
        {
            var gateway = new TimeEntryDataGateway(new TimeEntryContext(DbContextOptions));
            gateway.Create(22, 12, DateTime.Now, 8);

            // todo...
            var projectIds = Support.QuerySql("select project_id from time_entries");

            Assert.Equal(22L, projectIds[0]["project_id"]);
        }

        [Fact]
        public void TestFind()
        {
            Support.ExecSql(@"insert into time_entries (id, project_id, user_id, date, hours) 
values (2346, 22, 12, now(), 8);");

            var gateway = new TimeEntryDataGateway(new TimeEntryContext(DbContextOptions));
            var list = gateway.FindBy(12);

            // todo...
            var actual = list.First();
            Assert.Equal(2346, actual.Id);
            Assert.Equal(22, actual.ProjectId);
            Assert.Equal(12, actual.UserId);
        }
    }
}