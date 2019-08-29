using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Projects;
using Xunit;

namespace ProjectsTest
{
    public class ProjectControllerTest
    {
        private readonly Mock<IProjectDataGateway> _gateway;
        private readonly ProjectController _controller;

        public ProjectControllerTest()
        {
            _gateway = new Mock<IProjectDataGateway>();
            _controller = new ProjectController(_gateway.Object);
        }

        [Fact]
        public void TestPost()
        {
            _gateway.Setup(g => g.Create(1673, "aProject")).Returns(new ProjectRecord(123, 1673, "aProject", true));

            var response = _controller.Post(new ProjectInfo(-1, 1673, "aProject", true, ""));
            var body = (ProjectInfo) ((ObjectResult) response).Value;

            Assert.IsType<CreatedResult>(response);

            Assert.Equal(123, body.Id);
            Assert.Equal(1673, body.AccountId);
            Assert.Equal("aProject", body.Name);
            Assert.Equal(true, body.Active);
            Assert.Equal("project info", body.Info);
        }

        [Fact]
        public void TestGet()
        {
            _gateway.Setup(g => g.FindObject(55431)).Returns(new ProjectRecord(55431, 1673, "aProject", true));

            var response = _controller.Get(55431);
            var body = (ProjectInfo) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);
            
            Assert.Equal(55431, body.Id);
            Assert.Equal(1673, body.AccountId);
            Assert.Equal("aProject", body.Name);
            Assert.Equal(true, body.Active);
        }

        [Fact]
        public void TestGet_NotFound()
        {
            _gateway.Setup(g => g.FindObject(1673)).Returns(new ProjectRecord(55431, 1673, "aProject", true));

            var response = _controller.Get(99999);

            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void TestList()
        {
            _gateway.Setup(g => g.FindBy(1673)).Returns(new List<ProjectRecord>
            {
                new ProjectRecord(55431, 1673, "aProject", false),
                new ProjectRecord(55432, 1673, "anotherProject", true)
            });

            var response = _controller.List(1673);
            var body = (List<ProjectInfo>) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);

            Assert.Equal(2, ((List<ProjectInfo>) ((ObjectResult) response).Value).Count);
            
            Assert.Equal(55431, body[0].Id);
            Assert.Equal(1673, body[0].AccountId);
            Assert.Equal("aProject", body[0].Name);
            Assert.Equal(false, body[0].Active);
            
            Assert.Equal(55432, body[1].Id);
            Assert.Equal(1673, body[1].AccountId);
            Assert.Equal("anotherProject", body[1].Name);
            Assert.Equal(true, body[1].Active);
        }
    }
}