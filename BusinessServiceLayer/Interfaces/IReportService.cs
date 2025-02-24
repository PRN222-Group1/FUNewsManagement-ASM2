using BusinessServiceLayer.DTOs;

namespace BusinessServiceLayer.Interfaces
{
    public interface IReportService
    {
        Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticlesByDateRangeAsync(DateTime? startDate, DateTime? endDate);
        Task<int> GetCountNewsArticlesByDateRangeAsync(DateTime? startDate, DateTime? endDate);
    }
}
