namespace Logic.Statistics
{
    public interface IStatisticsHandler
    {
        Task<List<string>> GetSessionsForUser(int userId);
    }
}