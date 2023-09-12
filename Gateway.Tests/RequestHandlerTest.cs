using DAL.Interfaces;
using Logic.ExternalSystem;
using Logic.Interfaces;
using Logic.Logger;
using Logic.QueueLogic;
using Logic.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Gateway.Tests
{
    public class RequestHandlerTest
    {
        private readonly Mock<ICustomLogger<RequestHandler>> _loggerMock = new Mock<ICustomLogger<RequestHandler>>();
        private readonly Mock<IQueuesHandler> _queueHandler = new Mock<IQueuesHandler>();
        private readonly Mock<ICacheHandler> _cacheHandler = new Mock<ICacheHandler>();
        private readonly Mock<IRequestsRepository> repo = new Mock<IRequestsRepository>();
        private RequestHandler requestHandler;
        private DTOs.Request request;
        public RequestHandlerTest()
        {
            requestHandler = new RequestHandler(_cacheHandler.Object, repo.Object, _queueHandler.Object, _loggerMock.Object);
            request = new DTOs.Request()
            {
                RequestId = "dad",
                SessionId = "312",
                Datetime = DateTime.Now,
                UserId = 1
            };
            repo
             .Setup(m => m.RequestExists(It.IsAny<DTOs.Request>()))
             .Returns(Task.FromResult(false));
        }

        [Fact]
        public async void ShouldProcessRequestSuccessfully()
        {


            var bRes = await requestHandler.ProcessRequest(request);

            Assert.True(bRes);

            _loggerMock
                .Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async void ShouldNotProcessExistingRequestIdAndNotLogError()
        {
            repo
             .Setup(m => m.RequestExists(It.IsAny<DTOs.Request>()))
             .Returns(Task.FromResult(true));

            var bRes = await requestHandler.ProcessRequest(request);

            Assert.False(bRes);

            _loggerMock
                .Verify(x => x.LogError(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public async void ShouldNotProcessRequestAndLogErrorOnException()
        {
            repo
             .Setup(m => m.RequestExists(It.IsAny<DTOs.Request>()))
             .Throws(new ArgumentException("Error"));

            var bRes = await requestHandler.ProcessRequest(request);

            Assert.False(bRes);

            _loggerMock
                .Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);

        }
    }
}