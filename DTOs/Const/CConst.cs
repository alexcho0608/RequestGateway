using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Const
{
    public static class CConst
    {
        public static string QueueKey = "Queues";
        public static string QueuePrefix = "Q";
        public static string NumberOfExternalServicesAppKey = "NumberOfExternalServices";
        public static string LockRetrySecondsAppKey = "LockRetrySeconds";
        public static string LockExpiryTimeSecondsAppKey = "LockExpiryTimeSeconds";
        public static string WorkerSleepSecondsAppKey = "WorkerSleepSeconds";
        public static string RedisUrlAppKey = "RedisUrl";
        public static string ExternalServiceUrlAppKey = "ExternalServiceUrl";
    }
}
