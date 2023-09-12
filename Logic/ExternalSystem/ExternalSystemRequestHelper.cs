using DAL.Interfaces;
using DTOs.Const;
using Logic.Logger;
using Logic.QueueLogic;
using Logic.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Logic.ExternalSystem
{
    public class ExternalSystemRequestHelper : IExternalSystemRequestHelper
    {
        private ICustomLogger<ExternalSystemRequestHelper> _logger;
        private IQueuesHandler _queueHandler;
        private IExternalServiceResponseRepository _externalServiceResponseRepository;
        private IWebClient _webClient;
        private int sleepInSeconds;
        public ExternalSystemRequestHelper(IQueuesHandler queueHandler, IWebClient webClient,
            IExternalServiceResponseRepository repo, ICustomLogger<ExternalSystemRequestHelper> logger, IConfiguration config)
        {
            sleepInSeconds = config.GetValue<int>(CConst.WorkerSleepSecondsAppKey);
            _logger = logger;
            _externalServiceResponseRepository = repo;
            _queueHandler = queueHandler;
            _webClient = webClient;
        }

        public Task<int> CreateTasksForSendingRequestsToExternalService(int numberOfTasks)
        {
            var tasks = new Task[numberOfTasks];

            for (int i = 0; i < tasks.Length; i++)
            {
                int queueNumber = i;
                ++queueNumber;
                QueuesHandler.AddQueue(queueNumber);
                tasks[i] = Task.Run(() => ProcessRequestMessagesInQueue(queueNumber));
            }

            // Wait for all tasks to complete
            return Task.FromResult<int>(tasks.Count());
        }

        public async Task ProcessRequestMessagesInQueue(int queueNumber)
        {
            _logger.LogInformation($"Start worker number {queueNumber}");
            while (true)
            {

                await TryPopQueueMessage(queueNumber);
                await Task.Delay(sleepInSeconds*1000);
            }
        }

        public async Task TryPopQueueMessage(int queueNumber)
        {
            try
            {
                var request = await _queueHandler.PopRequest(queueNumber);
                if (request != null)
                {
                    _logger.LogInformation($"Processing poped object from queue {queueNumber}");

                    var message = await _webClient.SendRequest(request);
                    _logger.LogInformation($"Inserting message from external service: {message}");

                    await _externalServiceResponseRepository.Insert(message,request.RequestId);
                }
                {
                    _logger.LogInformation($"No objects to process from queue {queueNumber}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while sending message to external service{e.Message}");
            };
        }
    }
}
