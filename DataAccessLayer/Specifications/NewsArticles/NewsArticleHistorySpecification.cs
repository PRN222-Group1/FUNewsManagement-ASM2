using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.NewsArticles
{
    public class NewsArticleHistorySpecification : BaseSpecification<NewsArticle>
    {
        public NewsArticleHistorySpecification(int? accountId) : base(x =>
            !accountId.HasValue || x.CreatedById == accountId
        ) 
        {
            AddInclude(na => na.CreatedBy);
            AddInclude(na => na.Category);
        }
    }
}
