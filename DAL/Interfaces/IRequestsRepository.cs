using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRequestsRepository
    {
        Task InsertRequest(Request request);

        Task<IEnumerable<string>> GetSessions(int userId);

        Task<bool> RequestExists(DTOs.Request request);

    }
}
