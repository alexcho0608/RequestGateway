using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Statistics
{
    public class StatisticsHandler : IStatisticsHandler
    {
        private readonly IRequestsRepository _requestsRepository;
        public StatisticsHandler(IRequestsRepository repository)
        {
            _requestsRepository = repository;   
        }

        public async Task<List<string>> GetSessionsForUser(int userId)
        {
            var sessions = await _requestsRepository.GetSessions(userId);
            return sessions.ToList();
        }
    }
}
