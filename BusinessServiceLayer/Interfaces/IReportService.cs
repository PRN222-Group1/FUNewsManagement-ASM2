namespace BusinessServiceLayer.Interfaces
{
    public interface IReportService
    {
        Task<Dictionary<string, int>> GetArticlesCountByAuthorAsync(DateTime? startDate, DateTime? endDate);
        Task<int> CountPublishedNewsArticlesAsync(DateTime? startDate, DateTime? endDate);
        Task<int> CountDraftNewsArticlesAsync(DateTime? startDate, DateTime? endDate);
        Task<int> CountTotalNewsArticlesAsync(DateTime? startDate, DateTime? endDate);
    }
}
