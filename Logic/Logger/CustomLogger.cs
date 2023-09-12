using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Logger
{
    public class CustomLogger<T> : ICustomLogger<T>
    {
        private readonly ILogger _logger;
        public CustomLogger(ILogger<T> log)
        {
            _logger = log;
        }

        public void LogInformation(string message) 
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }


    }
}
