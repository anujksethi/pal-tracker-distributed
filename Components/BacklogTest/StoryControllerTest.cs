using System.Collections.Generic;
using System.Threading.Tasks;
using Backlog;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BacklogTest
{
    public class StoryControllerTest
    {
        private readonly Mock<IStoryDataGateway> _gateway;
        private readonly Mock<IProjectClient> _client;
        private readonly StoryController _controller;

        public StoryControllerTest()
        {
            _gateway = new Mock<IStoryDataGateway>();
            _client = new Mock<IProjectClient>();
            _controller = new StoryController(_gateway.Object, _client.Object);
        }

        [Fact]
        public void TestPost()
        {
            _gateway.Setup(g => g.Create(55432, "An epic story")).Returns(new StoryRecord(1234, 55432, "An epic story"));
            _client.Setup(c => c.Get(55432)).Returns(Task.FromResult(new ProjectInfo(true)));

            var response = _controller.Post(new StoryInfo(-1, 55432, "An epic story", ""));
            var body = (StoryInfo) ((ObjectResult) response).Value;

            Assert.IsType<CreatedResult>(response);

            Assert.Equal(1234, body.Id);
            Assert.Equal(55432, body.ProjectId);
            Assert.Equal("An epic story", body.Name);
            Assert.Equal("story info", body.Info);
        }

        [Fact]
        public void TestPost_InactiveProject()
        {
            _gateway.Setup(g => g.Create(55432, "An epic story")).Returns(new StoryRecord(1234, 55432, "An epic story"));
            _client.Setup(c => c.Get(55432)).Returns(Task.FromResult(new ProjectInfo(false)));

            var response = _controller.Post(new StoryInfo(-1, 55432, "An epic story", ""));

            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(304, ((StatusCodeResult) response).StatusCode);
        }

        [Fact]
        public void TestGet()
        {
            _gateway.Setup(g => g.FindBy(55432)).Returns(new List<StoryRecord>
            {
                new StoryRecord(1234, 55432, "An epic story"),
                new StoryRecord(5678, 55432, "An even more epic story")
            });

            var response = _controller.Get(55432);
            var body = (List<StoryInfo>) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);
            
            Assert.Equal(2, ((List<StoryInfo>) ((ObjectResult) response).Value).Count);

            Assert.Equal(1234, body[0].Id);
            Assert.Equal("An epic story", body[0].Name);
            Assert.Equal("story info", body[0].Info);

            Assert.Equal(5678, body[1].Id);
            Assert.Equal("An even more epic story", body[1].Name);
            Assert.Equal("story info", body[1].Info);
        }
    }
}