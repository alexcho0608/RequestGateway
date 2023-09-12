using DTOs.Const;
using Logic.Interfaces;
using Logic.Logger;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace Logic.QueueLogic
{
    public class QueuesHandler : IQueuesHandler
    {
        private static string _queuePrefix = CConst.QueuePrefix;
        private string _queueKey = CConst.QueueKey;
        private static Dictionary<string, ConcurrentQueue<DTOs.Request>> _dictOfQueues = new Dictionary<string, ConcurrentQueue<DTOs.Request>>();
        private readonly ICacheHandler _cacheHandler;
        private readonly ICustomLogger<QueuesHandler> _logger;

        public QueuesHandler(ICustomLogger<QueuesHandler> logger,ICacheHandler cacheHandler)
        {
            _logger = logger;
            _cacheHandler = cacheHandler;
        }

        public static void AddQueue(int queueNumber)
        {
            var name = _queuePrefix + queueNumber;
            if (!_dictOfQueues.ContainsKey(name))
            {
                _dictOfQueues.Add(name, new ConcurrentQueue<DTOs.Request>());
            }
        }

        public async Task<DTOs.Request> PopRequest(int queueNumber)
        {
            _logger.LogInformation($"Try to pop a request from queue {queueNumber}");
            var queuename = _queuePrefix + queueNumber;
            if (_dictOfQueues.ContainsKey(queuename) && _dictOfQueues[queuename].Count > 0)
            {
                using (var redLock = await _cacheHandler.Lock(_queueKey, queuename))
                {
                    var redisValue = await _cacheHandler.GetHashSetValue(_queueKey, queuename);
                    redisValue.TryParse(out int count);
                    count--;
                    await _cacheHandler.UpsertValue(_queueKey, queuename, count.ToString());
                    _dictOfQueues[queuename].TryDequeue(out var obj);
                    return obj;
                }
            }

            _logger.LogInformation($"No requests in queue {queuename} to process");

            return null;
        }

        public async Task AddRequest(DTOs.Request request)
        {
            var hashEntries = await _cacheHandler.GetHashSet(_queueKey);
            var minHashEntry = GetMinValueQueueEntry(hashEntries);
            if(minHashEntry != null && _dictOfQueues.ContainsKey(minHashEntry.Name))
            {
                using (var redLock = await _cacheHandler.Lock(_queueKey, minHashEntry.Name))
                {
                    var redisValue = await _cacheHandler.GetHashSetValue(_queueKey, minHashEntry.Name);
                    redisValue.TryParse(out int count);
                    count++;
                    await _cacheHandler.UpsertValue(_queueKey, minHashEntry.Name, count.ToString());
                    _dictOfQueues[minHashEntry.Name].Enqueue(request);
                }
            }
        }

        private HashEntry GetMinValueQueueEntry(HashEntry[] entries)
        {
            var minValue = int.MaxValue;
            var result = new HashEntry();
            foreach(var entry in entries)
            {
                entry.Value.TryParse(out int count);
                if (count < minValue)
                {
                    result = entry;
                    minValue = count;
                }
            }

            return result;
        }
    }
}
