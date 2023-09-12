using DTOs.Const;
using Logic.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ExternalSystem
{
    public class WebClient : IWebClient
    {
        private readonly ICustomLogger<WebClient> _logger;
        private readonly IConfiguration _config;
        private readonly string url;
        public WebClient(ICustomLogger<WebClient> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            url = _config.GetValue<string>(CConst.ExternalServiceUrlAppKey);
        }

        public async Task<string> SendRequest(object data)
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();

                        _logger.LogInformation("Response:");
                        _logger.LogInformation(responseBody);
                    }
                    else
                    {
                        _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                }

                return responseBody;
            }
        }
    }
}
