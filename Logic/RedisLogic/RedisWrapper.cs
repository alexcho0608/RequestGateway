using DTOs.Const;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic.RedisLogic
{
    public class RedisWrapper : IRedisWrapper
    {
        private IDatabase db;
        private ConnectionMultiplexer redis;
        private RedLockFactory redLockFactory;
        private int lockRetrySeconds;
        private int lockExpiryTimeSeconds;
        public RedisWrapper(IConfiguration config)
        {
            var url = config.GetValue<string>(CConst.RedisUrlAppKey);
            lockRetrySeconds = config.GetValue<int>(CConst.LockRetrySecondsAppKey);
            lockExpiryTimeSeconds = config.GetValue<int>(CConst.LockExpiryTimeSecondsAppKey);
            redis = ConnectionMultiplexer.Connect(url);
            db = redis.GetDatabase();
            var multiplexers = new List<RedLockMultiplexer>
            {
                redis
            };
            redLockFactory = RedLockFactory.Create(multiplexers);

        }

        public async Task<IRedLock> Lock(string key, string value)
        {
            var redLock = await redLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(lockExpiryTimeSeconds));
            while (!redLock.IsAcquired)
            {
                Thread.Sleep(TimeSpan.FromSeconds(lockRetrySeconds));
                redLock = await redLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(lockExpiryTimeSeconds));
            }

            return redLock;
        }

        public async Task<RedisValue> HashGetAsync(string key, string subkey)
        {
            return await db.HashGetAsync(key, subkey);
        }

        public async Task<bool> HashSetAsync(string key,string subkey, string value)
        {
            await db.HashSetAsync(key, subkey, value);

            return true;
        }

        public async Task<bool> KeyDeleteAsync(string key)
        {
            await db.KeyDeleteAsync(key);

            return true;
        }

        public async Task<HashEntry[]> GetHashEntries(string key)
        {
            var result = await db.HashGetAllAsync(key);
            return result;
        }

        public async Task UpsertHashSet(string key, HashEntry[] entries)
        {
            await db.HashSetAsync(key, entries);
        }

        ~RedisWrapper()
        {
            redis.Close();
        }
    }
}
