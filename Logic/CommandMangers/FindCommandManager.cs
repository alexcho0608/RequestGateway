using DAL.Interfaces;
using DTOs;
using DTOs.Models.XmlAPI;
using Logic.Interfaces;
using Logic.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.CommandMangers
{
    public class FindCommandManager : ICommandManager
    {

        private IRequestHandler _requestHandler;
        public FindCommandManager(IRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public bool ShouldReturnRequestIds { get => true; }

        public List<string> RequestIds { get; set; }
        public bool IsRequestProcessedSuccessfully { get; set; }

        public async Task DoAction(Command command)
        {
            var request = new DTOs.Request()
            {
                RequestId = command.Id,
                SessionId = command.Get.Session
            };

            IsRequestProcessedSuccessfully = await _requestHandler.ProcessRequest(request);
            RequestIds = await _requestHandler.GetRequestIds(request.SessionId);
        }
    }
}
