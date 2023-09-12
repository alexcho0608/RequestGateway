using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Logger
{
    public interface ICustomLogger<T>
    {
        void LogInformation(string message);

        void LogError(string message);
    }
}
