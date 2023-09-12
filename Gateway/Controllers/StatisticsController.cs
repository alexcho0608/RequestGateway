using Logic.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IStatisticsHandler _handler;

        public StatisticsController(ILogger<StatisticsController> logger,IStatisticsHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpGet]
        [Route("/statistics/get_sessions")]
        public async Task<IEnumerable<string>> GetSessions(int userId)
        {
            try
            {
                _logger.LogInformation($"Get sessions for user {userId}");
                var result = await _handler.GetSessionsForUser(userId);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while getting sessions: {e.Message}");
                return new List<string>();
            }
        }
    }
}