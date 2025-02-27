using BusinessObjects.Entities;
using BusinessObjects.Enums;
using BusinessServiceLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Specifications.Account;
using DataAccessLayer.Specifications.NewsArticles;

namespace BusinessServiceLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<NewsArticle> _newsArticleRepository;
        private readonly IGenericRepository<SystemAccount> _accountRepository;

        public ReportService(IGenericRepository<NewsArticle> newsArticleRepository,
            IGenericRepository<SystemAccount> accountRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _accountRepository = accountRepository;
        }

        public async Task<int> CountDraftNewsArticlesAsync(DateTime? startDate, DateTime? endDate)
        {

            var countSpec = new NewsArticleSpecification(false, startDate, endDate);
            var count = await _newsArticleRepository.CountAsync(countSpec);

            return count;
        }

        public async Task<int> CountPublishedNewsArticlesAsync(DateTime? startDate, DateTime? endDate)
        {
            var countSpec = new NewsArticleSpecification(true, startDate, endDate);
            var count = await _newsArticleRepository.CountAsync(countSpec);

            return count;
        }

        public async Task<int> CountTotalNewsArticlesAsync(DateTime? startDate, DateTime? endDate)
        {
            var countSpec = new NewsArticleCountSpecification(null, startDate, endDate);
            var count = await _newsArticleRepository.CountAsync(countSpec);
            return count;
        }

        public async Task<Dictionary<string, int>> GetArticlesCountByAuthorAsync(DateTime? startDate, DateTime? endDate)
        {
            // Create staff role params
            var staffParams = new AccountSpecParams();
            staffParams.Role = (int) Role.Staff;

            // Get all staffs
            var staffSpec = new AccountCountSpecification(staffParams);
            var staffs = await _accountRepository.ListAsync(staffSpec);

            var dictList = new Dictionary<string, int>();

            // Loop through all the staffs
            foreach (var staff in staffs)
            {
                // Count the number of articles created by the staff and add to dictList
                var newsArticleSpec = new NewsArticleCountSpecification(staff.Id, startDate, endDate);
                var newsArticlesCount = await _newsArticleRepository.CountAsync(newsArticleSpec);
                dictList.Add(staff.AccountName, newsArticlesCount);
            }

            return dictList;
        }
    }
}