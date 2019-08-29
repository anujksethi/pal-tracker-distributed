using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Timesheets;
using Xunit;

namespace TimesheetsTest
{
    public class TimeEntryControllerTest
    {
        private readonly Mock<ITimeEntryDataGateway> _gateway;
        private readonly Mock<IProjectClient> _client;
        private readonly TimeEntryController _controller;

        public TimeEntryControllerTest()
        {
            _gateway = new Mock<ITimeEntryDataGateway>();
            _client = new Mock<IProjectClient>();
            _controller = new TimeEntryController(_gateway.Object, _client.Object);
        }

        [Fact]
        public void TestPost()
        {
            _gateway.Setup(g => g.Create(55432, 4765, DateTime.Parse("2015-05-17"), 8))
                .Returns(new TimeEntryRecord(1234, 55432, 4765, DateTime.Parse("2015-05-17"), 8));

            _client.Setup(c => c.Get(55432)).Returns(Task.FromResult(new ProjectInfo(true)));

            var response = _controller.Post(new TimeEntryInfo(-1, 55432, 4765, DateTime.Parse("2015-05-17"), 8, ""));
            var body = (TimeEntryInfo) ((ObjectResult) response).Value;

            Assert.IsType<CreatedResult>(response);

            Assert.Equal(1234, body.Id);
            Assert.Equal(55432, body.ProjectId);
            Assert.Equal(4765, body.UserId);
            Assert.Equal(17, body.Date.Day);
            Assert.Equal(5, body.Date.Month);
            Assert.Equal(2015, body.Date.Year);
            Assert.Equal(8, body.Hours);
            Assert.Equal("entry info", body.Info);
        }

        [Fact]
        public void TestPost_InactiveProject()
        {
            _gateway.Setup(g => g.Create(55432, 4765, DateTime.Parse("2015-05-17"), 8))
                .Returns(new TimeEntryRecord(1234, 55432, 4765, DateTime.Parse("2015-05-17"), 8));

            _client.Setup(c => c.Get(55432)).Returns(Task.FromResult(new ProjectInfo(false)));

            var response = _controller.Post(new TimeEntryInfo(-1, 55432, 4765, DateTime.Parse("2015-05-17"), 8, ""));

            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(304, ((StatusCodeResult) response).StatusCode);
        }

        [Fact]
        public void TestGet()
        {
            _gateway.Setup(g => g.FindBy(4765)).Returns(new List<TimeEntryRecord>
            {
                new TimeEntryRecord(1234, 55432, 4765, DateTime.Parse("2015-05-17"), 8),
                new TimeEntryRecord(5678, 55433, 4765, DateTime.Parse("2015-05-18"), 6)
            });

            var response = _controller.Get(4765);
            var body = (List<TimeEntryInfo>) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);

            Assert.Equal(2, ((List<TimeEntryInfo>) ((ObjectResult) response).Value).Count);

            Assert.Equal(1234, body[0].Id);
            Assert.Equal(55432, body[0].ProjectId);
            Assert.Equal(4765, body[0].UserId);
            Assert.Equal(17, body[0].Date.Day);
            Assert.Equal(5, body[0].Date.Month);
            Assert.Equal(2015, body[0].Date.Year);
            Assert.Equal(8, body[0].Hours);
            Assert.Equal("entry info", body[0].Info);

            Assert.Equal(5678, body[1].Id);
            Assert.Equal(55433, body[1].ProjectId);
            Assert.Equal(4765, body[1].UserId);
            Assert.Equal(18, body[1].Date.Day);
            Assert.Equal(5, body[1].Date.Month);
            Assert.Equal(2015, body[1].Date.Year);
            Assert.Equal(6, body[1].Hours);
            Assert.Equal("entry info", body[1].Info);
        }
    }
}