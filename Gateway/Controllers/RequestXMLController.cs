using DTOs.Models.XmlAPI;
using Gateway.Controllers;
using Logic.CommandMangers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestXMLController : ControllerBase
    {
        private readonly ILogger<RequestXMLController> _logger;
        private readonly IFactory _factory;
        public RequestXMLController(ILogger<RequestXMLController> logger, IFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        [HttpPost]
        [Route("/xml_api/command")]
        [Consumes("application/xml")]
        public async Task<IActionResult> Insert(Command command)
        {
            try
            {
                _logger.LogInformation($"Executing command {command.Id}");
                var manager = _factory.GetManager(command);
                await manager.DoAction(command);

                if (manager.ShouldReturnRequestIds)
                {
                    return Ok(manager.RequestIds);
                }

                if (!manager.IsRequestProcessedSuccessfully)
                {
                    return BadRequest("There was an issue processing the request");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while executing xml command: {e.Message}");
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
