namespace Logic.RedisLogic
{
    using Logic.Interfaces;
    using Logic.Logger;
    using Microsoft.Extensions.Logging;
    using RedLockNet;
    using StackExchange.Redis;

    public class RedisCacheHandler : ICacheHandler
    {
        private readonly IRedisWrapper _redisWrapper;
        private readonly ICustomLogger<RedisCacheHandler> _logger;
        public RedisCacheHandler(ICustomLogger<RedisCacheHandler> logger, IRedisWrapper redisWrapper)
        {
            _logger = logger; 
            _redisWrapper = redisWrapper;
        }

        public async Task<bool> HashSetValueExists(string key, string subkey)
        {
            _logger.LogInformation($"Check if key {key} with subkey {subkey} exists");
            var val = await _redisWrapper.HashGetAsync(key,subkey);
            if (val == RedisValue.Null) return false;
            return true;
        }

        public async Task<IRedLock> Lock(string key, string subkey)
        {
            return await _redisWrapper.Lock(key, subkey);
        }

        public async Task<RedisValue> GetHashSetValue(string key, string subkey)
        {
            _logger.LogInformation($"Get hash set value {key} exists");
            var val = await _redisWrapper.HashGetAsync(key, subkey);
            
            return val;
        }

        public async Task<bool> UpsertValue(string key, string subkey, string value)
        {
            _logger.LogInformation($"Set key {key} with new hash subvalue {subkey} ");

            // Set a key-value pair
            await _redisWrapper.HashSetAsync(key, subkey, value);

            return true;
        }

        public async Task<bool> DeleteValue(string key)
        {
            _logger.LogInformation($"Delete key {key} ");

            await _redisWrapper.KeyDeleteAsync(key);

            return true;
        }

        public async Task<List<string>> GetHashSetSubKeys(string key)
        {
            _logger.LogInformation($"Get hash set subkeys for key {key} ");

            var entries = await _redisWrapper.GetHashEntries(key);
            var result = entries.Select(e => e.Name.ToString()).ToList();
            return result;
        }

        public async Task<HashEntry[]> GetHashSet(string key)
        {
            _logger.LogInformation($"Get hash set for key {key} ");

            var entries = await _redisWrapper.GetHashEntries(key);
            return entries;
        }

        public async Task UpsertHashSet(string key, Dictionary<string,string> entries)
        {
            var count = entries.Count;
            HashEntry[] hashEntries = new HashEntry[count];
            int i = 0;
            foreach(var pair in entries)
            {
                hashEntries[i] = new HashEntry(pair.Key, pair.Value);
                i++;
            }

            await _redisWrapper.UpsertHashSet(key, hashEntries);
        }
    }
}