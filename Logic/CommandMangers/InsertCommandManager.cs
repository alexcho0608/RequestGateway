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
    public class InsertCommandManager : ICommandManager
    {
        private IRequestHandler _requestHandler;

        public InsertCommandManager(IRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public bool ShouldReturnRequestIds { get => false; }

        public List<string> RequestIds { get; set; }

        public bool IsRequestProcessedSuccessfully { get; set; }

        public async Task DoAction(Command command)
        {
            var request = new DTOs.Request()
            {
                RequestId = command.Id,
                SessionId = command.Enter.Session,
                Datetime = DateTimeOffset.FromUnixTimeMilliseconds(command.Enter.Timestamp).DateTime,
                UserId = command.Enter.Player
            };

            IsRequestProcessedSuccessfully = await _requestHandler.ProcessRequest(request);

        }
    }
}
