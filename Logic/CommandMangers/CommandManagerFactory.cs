using DTOs.Models.XmlAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.CommandMangers
{
    public class CommandManagerFactory : IFactory
    {
        private readonly IEnumerable<ICommandManager> _managers;

        public CommandManagerFactory(IEnumerable<ICommandManager> mannagers)
        {
            _managers = mannagers;
        }

        public ICommandManager GetManager(Command command)
        {
            if (command.Enter != null)
            {
                var manager = GetManager(typeof(InsertCommandManager));
                return manager;
            }
            else if(command.Get != null)
            {
                var manager = GetManager(typeof(FindCommandManager));
                return manager;
            }

            throw new ArgumentException("Could not resolve the command to the appropriate manager");
        }

        private ICommandManager GetManager(Type type)
        {
            var manager = _managers.Where(e => e.GetType() == type).FirstOrDefault();
            return manager;
        }
    }
}
