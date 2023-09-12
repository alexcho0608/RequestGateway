using DAL.Interfaces;
using DTOs;
using Logic.ExternalSystem;
using Logic.Interfaces;
using Logic.Logger;
using Logic.QueueLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace Gateway.Tests
{
    public class QueueHandlerTest
    {
        private readonly Mock<ICustomLogger<QueuesHandler>> _loggerMock = new Mock<ICustomLogger<QueuesHandler>>();
        private readonly Mock<ICacheHandler> _cacheHandler = new Mock<ICacheHandler>();
        private DTOs.Request request;

        private QueuesHandler queueHandler;
        public QueueHandlerTest()
        {
            queueHandler = new QueuesHandler(_loggerMock.Object,_cacheHandler.Object);
            request = new DTOs.Request()
            {
                RequestId = "dad",
                SessionId = "312",
                Datetime = DateTime.Now,
                UserId = 1
            };
        }

        [Fact]
        public async void EmptyQueueShouldReturnNull()
        {
            QueuesHandler.AddQueue(1);
            var res = await queueHandler.PopRequest(1);
            Assert.Null(res);
        }


        [Fact]
        public async void AddRequestThenPopShouldReturnAnObject()
        {
            var hashEntries = new HashEntry[] { new HashEntry("Q1", 4), new HashEntry("Q2", 1) };
            _cacheHandler
             .Setup(m => m.GetHashSet(It.IsAny<string>()))
             .Returns(Task.FromResult(hashEntries));

            QueuesHandler.AddQueue(1);
            QueuesHandler.AddQueue(2);
            //request should be added to Q2, because it has less elements
            await queueHandler.AddRequest(request);
            var res = await queueHandler.PopRequest(1);
            Assert.Null(res);
            res = await queueHandler.PopRequest(2);
            Assert.NotNull(res);
        }
    }
}