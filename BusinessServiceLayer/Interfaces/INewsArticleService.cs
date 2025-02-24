using BusinessServiceLayer.DTOs;
using DataAccessLayer.Specifications.NewsArticles;

namespace BusinessServiceLayer.Interfaces
{
    public interface INewsArticleService
    {
        Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticlesAsync(NewsArticleSpecParams search);
        Task<int> CountNewsArticlesAsync(NewsArticleSpecParams specParams);
        Task<NewsArticleDTO> GetNewsArticleByIdAsync(int newsId);
        Task<IReadOnlyList<CategoryDTO>> GetAllCategories();
        Task<bool> CreateNewsArticleAsync(NewsArticleToAddOrUpdateDTO newsArticle);
        Task<bool> UpdateNewsArticleAsync(int id, NewsArticleToAddOrUpdateDTO newsArticle);
        Task<bool> DeleteNewsArticleAsync(int id);
        Task<IReadOnlyList<TagDTO>> GetAllTags();
        Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticleHistoryAsync(int accountId);
    }
}
