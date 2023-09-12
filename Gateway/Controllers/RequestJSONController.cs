using DTOs.Models.JsonAPI;
using DTOs.Models.StatisticsAPI;
using Logic.Request;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    public class RequestJSONController : ControllerBase
    {

        private readonly ILogger<RequestJSONController> _logger;
        private readonly IRequestHandler _requestHandler;

        public RequestJSONController(ILogger<RequestJSONController> logger, IRequestHandler requestHandler)
        {
            _logger = logger;
            _requestHandler = requestHandler;
        }


        [HttpPost]
        [Route("/json_api/insert")]
        public async Task<IActionResult> Insert(InsertRequest request)
        {
            try
            {
                _logger.LogInformation($"Inserting request with id {request.RequestId}");
                var req = new DTOs.Request()
                {
                    RequestId = request.RequestId,
                    SessionId = request.SessionId,
                    Datetime = DateTimeOffset.FromUnixTimeMilliseconds(request.Timestamp).DateTime,
                    UserId = request.ProducerId
                };

                var bRes = await _requestHandler.ProcessRequest(req);

                if(!bRes)
                    return BadRequest("Error inserting request");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error inserting request {e.Message}");
                return BadRequest("Error inserting request");
            }

            return Ok();
        }

        [HttpPost]
        [Route("/json_api/find")]
        public async Task<IEnumerable<string>> GetRequestList(GetRequestListRequest request)
        {
            try
            {
                _logger.LogInformation($"Finding requests with id {request.RequestId}");
                var req = new DTOs.Request()
                {
                    RequestId = request.RequestId,
                    SessionId = request.SessionId
                };

                await _requestHandler.ProcessRequest(req);
                var list = await _requestHandler.GetRequestIds(req.SessionId);
                return list;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error inserting request {e.Message}");
            }

            return new List<string>();
        }
    }
}