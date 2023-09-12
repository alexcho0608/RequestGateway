using DAL.Interfaces;
using DTOs;
using Logic.Interfaces;
using Logic.Logger;
using Logic.QueueLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Request
{
    public class RequestHandler : IRequestHandler
    {
        private IRequestsRepository _repository;
        private ICacheHandler _cacheHandler;
        private IQueuesHandler _queueHandler;
        private ICustomLogger<RequestHandler> _logger;
        public RequestHandler(ICacheHandler cacheHandler, IRequestsRepository repository, IQueuesHandler queueHandler, ICustomLogger<RequestHandler> logger)
        {
            _repository = repository;
            _cacheHandler = cacheHandler;
            _queueHandler = queueHandler;
            _logger = logger;
        }

        public async Task<List<string>> GetRequestIds(string sessionId)
        {
            _logger.LogInformation($"Get request ids for session from cache {sessionId}");

            return await _cacheHandler.GetHashSetSubKeys(sessionId);
        }

        public async Task<bool> ProcessRequest(DTOs.Request request)
        {
            _logger.LogInformation($"Processing request with id {request.RequestId}");
            try
            {
                if (!await _repository.RequestExists(request))
                {
                    await _repository.InsertRequest(request);

                    await _cacheHandler.UpsertValue(request.SessionId, request.RequestId, "");

                    await _queueHandler.AddRequest(request);

                }
                else
                {
                    _logger.LogInformation($"Request id {request.RequestId} already exists");
                    return false;
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"An error occured - message: {e.Message}");
                return false;
            }

            return true;
        }
    }
}
