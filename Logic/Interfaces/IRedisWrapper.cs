using RedLockNet;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IRedisWrapper
    {
        Task<RedisValue> HashGetAsync(string key, string value);
        Task<bool> HashSetAsync(string key, string subkey, string value);

        Task<bool> KeyDeleteAsync(string key);

        Task<IRedLock> Lock(string key, string value);

        Task<HashEntry[]> GetHashEntries(string key);

        Task UpsertHashSet(string key, HashEntry[] entries);
    }
}
