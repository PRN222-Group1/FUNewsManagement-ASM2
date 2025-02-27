using BusinessObjects.Entities;
using Microsoft.Identity.Client;

namespace DataAccessLayer.Specifications.NewsArticles
{
    public class NewsArticleCountSpecification : BaseSpecification<NewsArticle>
    {
        public NewsArticleCountSpecification(NewsArticleSpecParams specParams)
            : base(x => (string.IsNullOrEmpty(specParams.Search)
            || x.Headline.Contains(specParams.Search)
            || x.NewsTitle.Contains(specParams.Search)
            || x.CreatedBy.AccountName.Contains(specParams.Search))
            && (!specParams.CatId.HasValue
            || x.CategoryId == specParams.CatId)
            && (!specParams.Status.HasValue || x.NewsStatus == specParams.Status)
            )
        {
        }

        public NewsArticleCountSpecification(int? accountId, DateTime? startDate, DateTime? endDate) : base(x =>
            (!accountId.HasValue || x.CreatedById == accountId)
            && (!(startDate.HasValue && endDate.HasValue)
            || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
        )
        {
        }
    }
}
