using RedLockNet;
using StackExchange.Redis;

namespace Logic.Interfaces
{
    public interface ICacheHandler
    {
        Task<bool> UpsertValue(string key, string subkey, string value);

        Task<bool> DeleteValue(string key);

        Task UpsertHashSet(string key, Dictionary<string,string> entries);

        Task<bool> HashSetValueExists(string key, string value);
        Task<List<string>> GetHashSetSubKeys(string key);
        Task<HashEntry[]> GetHashSet(string key);
        Task<IRedLock> Lock(string key, string subkey);
        Task<RedisValue> GetHashSetValue(string key, string subkey);
    }
}