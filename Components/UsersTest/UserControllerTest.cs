using Microsoft.AspNetCore.Mvc;
using Moq;
using Users;
using Xunit;

namespace UsersTest
{
    public class UserControllerTest
    {
        private readonly Mock<IUserDataGateway> _gateway;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _gateway = new Mock<IUserDataGateway>();
            _controller = new UserController(_gateway.Object);
        }

        [Fact]
        public void TestGet()
        {
            _gateway.Setup(g => g.FindObjectBy(4765)).Returns(new UserRecord(4765, "Jack"));

            var response = _controller.Get(4765);
            var body = (UserInfo) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(4765, body.Id);
            Assert.Equal("Jack", body.Name);
            Assert.Equal("user info", body.Info);
        }
        
        [Fact]
        public void TestGet_NotFound()
        {
            _gateway.Setup(g => g.FindObjectBy(4765)).Returns(new UserRecord(4765, "Jack"));

            var response = _controller.Get(9999);

            Assert.IsType<NotFoundResult>(response);
        }
    }
}