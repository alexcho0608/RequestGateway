namespace Logic.ExternalSystem
{
    public interface IWebClient
    {
        Task<string> SendRequest(object data);
    }
}