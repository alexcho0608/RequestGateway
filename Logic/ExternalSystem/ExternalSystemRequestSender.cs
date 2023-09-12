using DAL.Interfaces;
using DTOs.Const;
using Logic.Interfaces;
using Logic.Logger;
using Logic.QueueLogic;
using Logic.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ExternalSystem
{
    public class ExternalSystemRequestSender : BackgroundService
    {
        private int numberOfExternalServices;
        private IExternalSystemRequestHelper _externalServiceRequestHelper;
        private ICustomLogger<ExternalSystemRequestSender> _logger;
        private ICacheHandler cacheHandler;
        public ExternalSystemRequestSender(IServiceProvider provider, IConfiguration config)
        {
            var scope = provider.CreateScope();
            numberOfExternalServices = config.GetValue<int>(CConst.NumberOfExternalServicesAppKey);
            _logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<ExternalSystemRequestSender>>();
            _externalServiceRequestHelper = scope.ServiceProvider.GetRequiredService<IExternalSystemRequestHelper>();
            cacheHandler = scope.ServiceProvider.GetRequiredService<ICacheHandler>();
        }
      

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CreateQueueHashSetInCache(numberOfExternalServices);

            _logger.LogInformation("Start tasks");
            var res = _externalServiceRequestHelper.CreateTasksForSendingRequestsToExternalService(numberOfExternalServices);

            return res;
        }

        private void CreateQueueHashSetInCache(int numberOfQueues)
        {
            _logger.LogInformation("Creating cache key for queues");

            Dictionary<string,string> hashEntries = new Dictionary<string,string>();

            for(var i = 1; i <= numberOfQueues;i++)
            {
                hashEntries.Add(CConst.QueuePrefix + i, "0");
            }

            cacheHandler.UpsertHashSet(CConst.QueueKey, hashEntries);
        }
    }
}
