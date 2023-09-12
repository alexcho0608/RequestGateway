using DAL.Interfaces;
using Dapper;
using DTOs;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ExternalServiceResponseRepository : IExternalServiceResponseRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly ILogger<ExternalServiceResponseRepository> _logger;

        public ExternalServiceResponseRepository(ILogger<ExternalServiceResponseRepository> logger, IDBConnection dbConnection)
        {
            _connection = dbConnection.GetConnection();
            _logger = logger;
        }

        public async Task Insert(string message,string requestId)
        {

            try
            {
                var parameters = new { Message = message,RequestId = requestId };
                await _connection.ExecuteAsync($"INSERT INTO dbo.\"ExternalServiceResponses\" (\"Message\",\"RequestId\") VALUES (@Message,@RequestId)",parameters);

            }
            catch (Exception e)
            {
                _logger.LogError($"Error while inserting message from external service {e.Message}");
            }
        }
    }
}
