using DAL.Interfaces;
using Dapper;
using DTOs;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RequestRepository : IRequestsRepository
    {

        private readonly NpgsqlConnection _connection;
        private readonly ILogger<RequestRepository> _logger;


        public RequestRepository(ILogger<RequestRepository> logger, IDBConnection dbConnection)
        {
            _connection = dbConnection.GetConnection();
            _logger = logger;
        }

        ~RequestRepository()
        {
            _connection.Close();
        }

        public async Task<IEnumerable<string>> GetSessions(int userId)
        {
            var result = new List<string>();
            try
            {
                // Use Dapper to query the database
                result = (await _connection.QueryAsync<string>($"SELECT DISTINCT (\"SessionId\") FROM dbo.\"Requests\" WHERE \"UserId\" = @UserId",new { UserId = userId })).ToList();

            }
            catch (Exception e)
            {
                _logger.LogError($"Error while instering request {e.Message}");
                await _connection.CloseAsync();
            }
            return result;
        }


        public async Task InsertRequest(Request request)
        {
            try
            {
                    var parameters = new { RequestId = request.RequestId, SessionId = request.SessionId,
                    Timestamp = request.Datetime, UserId = request.UserId};
                    await _connection.ExecuteAsync($"INSERT INTO dbo.\"Requests\" VALUES " +
                        $"(@RequestId,@UserId,@SessionId,@Timestamp)", parameters);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while instering request {e.Message}");
                throw;
            }
        }

        public async Task<bool> RequestExists(Request request)
        {
            var parameters = new { RequestId = request.RequestId };
            var res = await _connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM dbo.\"Requests\" WHERE \"RequestId\" = @RequestId",parameters);
            return res > 0;
        }
    }
}
