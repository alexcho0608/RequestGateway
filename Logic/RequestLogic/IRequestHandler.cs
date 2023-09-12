using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Request
{
    public interface IRequestHandler
    {
        Task<bool> ProcessRequest(DTOs.Request request);
        Task<List<string>> GetRequestIds(string sessionId);
    }
}
