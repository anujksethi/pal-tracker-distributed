using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Allocations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AllocationsTest
{
    public class AllocationsControllerTest
    {
        private readonly Mock<IAllocationDataGateway> _gateway;
        private readonly Mock<IProjectClient> _client;
        private readonly AllocationController _controller;

        public AllocationsControllerTest()
        {
            _gateway = new Mock<IAllocationDataGateway>();
            _client = new Mock<IProjectClient>();
            _controller = new AllocationController(_gateway.Object, _client.Object);
        }

        [Fact]
        public void TestPost()
        {
            _gateway.Setup(g => g.Create(55432, 4765, DateTime.Parse("2014-05-16"), DateTime.Parse("2014-05-26")))
                .Returns(new AllocationRecord(3, 55432, 4765, DateTime.Parse("2014-05-16"), DateTime.Parse("2014-05-26")));

            _client.Setup(c => c.Get(55432)).Returns(Task.FromResult(new ProjectInfo(true)));

            var response = _controller.Post(new AllocationInfo(-1, 55432, 4765, DateTime.Parse("2014-05-16"), DateTime.Parse("2014-05-26"), ""));
            var body = (AllocationInfo) ((ObjectResult) response).Value;

            Assert.IsType<CreatedResult>(response);
            
            Assert.Equal(3, body.Id);
            Assert.Equal(55432L, body.ProjectId);
            Assert.Equal(4765L, body.UserId);
            Assert.Equal(16, body.FirstDay.Day);
            Assert.Equal(5, body.FirstDay.Month);
            Assert.Equal(2014, body.FirstDay.Year);
            Assert.Equal(26, body.LastDay.Day);
            Assert.Equal(5, body.FirstDay.Month);
            Assert.Equal(2014, body.FirstDay.Year);
            Assert.Equal("allocation info", body.Info);
        }

        [Fact]
        public void TestGet()
        {
            _gateway.Setup(g => g.FindBy(55432)).Returns(new List<AllocationRecord>
            {
                new AllocationRecord(754, 55432, 4765, DateTime.Parse("2015-05-16"), DateTime.Parse("2015-05-17")),
                new AllocationRecord(755, 55432, 4766, DateTime.Parse("2015-05-17"), DateTime.Parse("2015-05-18"))
            });

            _client.Setup(c => c.Get(55432)).Returns(Task.FromResult(new ProjectInfo(true)));

            var response = _controller.Get(55432);
            var body = (List<AllocationInfo>) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);

            Assert.Equal(2, body.Count);

            Assert.Equal(754L, body[0].Id);
            Assert.Equal(55432L, body[0].ProjectId);
            Assert.Equal(4765L, body[0].UserId);
            Assert.Equal(16, body[0].FirstDay.Day);
            Assert.Equal(5, body[0].FirstDay.Month);
            Assert.Equal(2015, body[0].FirstDay.Year);
            Assert.Equal(17, body[0].LastDay.Day);
            Assert.Equal(5, body[0].FirstDay.Month);
            Assert.Equal(2015, body[0].FirstDay.Year);
            Assert.Equal("allocation info", body[0].Info);

            Assert.Equal(755L, body[1].Id);
            Assert.Equal(55432L, body[1].ProjectId);
            Assert.Equal(4766L, body[1].UserId);
            Assert.Equal(17, body[1].FirstDay.Day);
            Assert.Equal(5, body[1].FirstDay.Month);
            Assert.Equal(2015, body[1].FirstDay.Year);
            Assert.Equal(18, body[1].LastDay.Day);
            Assert.Equal(5, body[1].FirstDay.Month);
            Assert.Equal(2015, body[1].FirstDay.Year);
            Assert.Equal("allocation info", body[1].Info);
        }
    }
}