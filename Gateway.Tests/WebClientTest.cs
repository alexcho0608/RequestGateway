using Logic.ExternalSystem;
using Logic.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gateway.Tests
{
    public class WebClientTest
    {
        private readonly Mock<ICustomLogger<WebClient>> _loggerMock = new Mock<ICustomLogger<WebClient>>();
        private WebClient client;
        public WebClientTest()
        {
        }

        [Fact]
        public async void Post_ShouldWork()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"ExternalServiceUrl", "https://postman-echo.com/get"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            client = new WebClient(_loggerMock.Object, configuration);

            //When IC data is missing
            await client.SendRequest(new object());

            _loggerMock
                .Verify(x => x.LogInformation(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public async void Post_WrongUrl_ShouldFail()
        {
            //When IC data is missing
            var inMemorySettings = new Dictionary<string, string> {
                {"ExternalServiceUrl", "d"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            client = new WebClient(_loggerMock.Object, configuration);
            await client.SendRequest(new object());

            _loggerMock
                .Verify(x => x.LogError(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}