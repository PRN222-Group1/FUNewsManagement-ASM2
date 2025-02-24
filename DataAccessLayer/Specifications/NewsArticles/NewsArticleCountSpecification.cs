using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.NewsArticles
{
    public class NewsArticleCountSpecification : BaseSpecification<NewsArticle>
    {
        public NewsArticleCountSpecification(NewsArticleSpecParams specParams)
            : base(x => (string.IsNullOrEmpty(specParams.Search)
            || x.Headline.Contains(specParams.Search)
            || x.NewsTitle.Contains(specParams.Search)
            || x.CreatedBy.AccountName.Contains(specParams.Search))
            && (!specParams.CategoryId.HasValue
            || x.CategoryId == specParams.CategoryId)
            && (!specParams.Status.HasValue || x.NewsStatus == specParams.Status)
            )
        {
        }

        public NewsArticleCountSpecification(DateTime? startDate, DateTime? endDate) : base(x =>
            !(startDate.HasValue && endDate.HasValue)
            || (x.CreatedDate >= startDate && x.CreatedDate <= endDate)
        )
        {
        }
    }
}
