using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.QueueLogic
{
    public interface IQueuesHandler
    {
        Task<DTOs.Request> PopRequest(int queueNumber);

        Task AddRequest(DTOs.Request request);
    }
}
