namespace Logic.ExternalSystem
{
    public interface IExternalSystemRequestHelper
    {
        Task<int> CreateTasksForSendingRequestsToExternalService(int numberOfTasks);

        Task TryPopQueueMessage(int queueNumber);
    }
}