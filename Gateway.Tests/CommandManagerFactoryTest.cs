using DAL.Interfaces;
using DTOs.Models.XmlAPI;
using Logic.CommandMangers;
using Logic.ExternalSystem;
using Logic.Logger;
using Logic.QueueLogic;
using Logic.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gateway.Tests
{
    public class CommandManagerFactoryTest
    {

        private readonly Mock<IRequestHandler> _requestHandler = new Mock<IRequestHandler>();
        private readonly List<ICommandManager> managerList = new List<ICommandManager>();
        private CommandManagerFactory factory;
        public CommandManagerFactoryTest()
        {
            managerList.Add(new FindCommandManager(_requestHandler.Object));
            managerList.Add(new InsertCommandManager(_requestHandler.Object));
            factory = new CommandManagerFactory(managerList);
        }

        [Fact]
        public async void ShouldReturnInsertCommandManager()
        {
            var command = new Command()
            {
                Id = "1",
                Enter = new Enter()
            };

            var res = factory.GetManager(command);

            Assert.Equal(typeof(InsertCommandManager), res.GetType());
        }



        [Fact]
        public async void ShouldReturnFindCommandManager()
        {
            var command = new Command()
            {
                Id = "1",
                Get = new Get()
            };

            var res = factory.GetManager(command);

            Assert.Equal(typeof(FindCommandManager), res.GetType());
        }


    }
}