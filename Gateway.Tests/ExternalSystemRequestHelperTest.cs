using DAL.Interfaces;
using Logic.ExternalSystem;
using Logic.Logger;
using Logic.QueueLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gateway.Tests
{
    public class ExternalSystemRequestHelperTest
    {
        private readonly Mock<ICustomLogger<ExternalSystemRequestHelper>> _loggerMock = new Mock<ICustomLogger<ExternalSystemRequestHelper>>();
        private readonly Mock<IQueuesHandler> _queueHandler = new Mock<IQueuesHandler>();
        private readonly Mock<IWebClient> _webClient = new Mock<IWebClient>();
        private readonly Mock<IExternalServiceResponseRepository> repo = new Mock<IExternalServiceResponseRepository>();
        private ExternalSystemRequestHelper helper;
        public ExternalSystemRequestHelperTest()
        {

            var inMemorySettings = new Dictionary<string, string> {
                {"WorkerSleepSeconds", "3"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _queueHandler
                 .Setup(m => m.PopRequest(It.IsAny<int>()))
                 .Returns(Task.FromResult(new DTOs.Request()
                 {
                     UserId = 1,
                     RequestId = "132213",
                     SessionId = "dwwa",
                     Datetime = DateTime.Now
                 })) ;
            helper = new ExternalSystemRequestHelper(_queueHandler.Object,_webClient.Object, repo.Object, _loggerMock.Object, configuration);
        }

        [Fact]
        public async void ShouldCreateFourTasksShouldSucceed()
        {
            int numberOfTasks = 4;
            var res = helper.CreateTasksForSendingRequestsToExternalService(numberOfTasks);
            Assert.Equal(numberOfTasks, await res);
        }


        [Fact]
        public async void PopRequestFromQueueShouldSucceed()
        {
            int queueNumber = 1;

            await helper.TryPopQueueMessage(queueNumber);
            _loggerMock
                .Verify(x => x.LogError(It.IsAny<string>()), Times.Never);

        }
    }
}