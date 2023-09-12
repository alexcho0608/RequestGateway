using DTOs;
using DTOs.Models.XmlAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.CommandMangers
{
    public interface ICommandManager
    {
        Task DoAction(Command request);

        bool ShouldReturnRequestIds { get; }

        bool IsRequestProcessedSuccessfully { get; set; }

        public List<string> RequestIds { get; set; }
    }
}
