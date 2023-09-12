using DTOs.Models.XmlAPI;

namespace Logic.CommandMangers
{
    public interface IFactory
    {
        ICommandManager GetManager(Command command);
    }
}