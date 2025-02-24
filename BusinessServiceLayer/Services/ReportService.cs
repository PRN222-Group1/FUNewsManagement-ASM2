using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using BusinessObjects.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Specifications.NewsArticles;

namespace BusinessServiceLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<NewsArticle> _repository;
        private readonly IMapper _mapper;

        public ReportService(IGenericRepository<NewsArticle> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticlesByDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            var spec = new NewsArticleSpecification(startDate, endDate);
            var newsArticle = await _repository.ListAsync(spec);
            return _mapper.Map<IReadOnlyList<NewsArticle>, IReadOnlyList<NewsArticleDTO>>(newsArticle);
        }

        public async Task<int> GetCountNewsArticlesByDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            var spec = new NewsArticleCountSpecification(startDate, endDate);
            var count = await _repository.CountAsync(spec);
            return count;
        }
    }
}