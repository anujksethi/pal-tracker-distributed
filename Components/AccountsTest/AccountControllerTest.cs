using System.Collections.Generic;
using Accounts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AccountsTest
{
    public class AccountControllerTest
    {
        private readonly Mock<IAccountDataGateway> _gateway;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            _gateway = new Mock<IAccountDataGateway>();
            _controller = new AccountController(_gateway.Object);
        }

        [Fact]
        public void TestGet()
        {
            _gateway.Setup(g => g.FindBy(4765)).Returns(new List<AccountRecord>
            {
                new AccountRecord(1673, 4765, "Jack's account"),
                new AccountRecord(1674, 4765, "Jack's other account")
            });

            var response = _controller.Get(4765);
            var body = (List<AccountInfo>) ((ObjectResult) response).Value;

            Assert.IsType<OkObjectResult>(response);

            Assert.Equal(2, body.Count);
            Assert.Equal(1673, body[0].Id);
            Assert.Equal(4765, body[0].OwnerId);
            Assert.Equal("Jack's account", body[0].Name);
            Assert.Equal("account info", body[0].Info);
            Assert.Equal(1674, body[1].Id);
            Assert.Equal(4765, body[1].OwnerId);
            Assert.Equal("Jack's other account", body[1].Name);
            Assert.Equal("account info", body[1].Info);
        }
    }
}