﻿using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.NewsArticles
{
    public class NewsArticleSpecification : BaseSpecification<NewsArticle>
    {
        public NewsArticleSpecification(NewsArticleSpecParams specParams) 
            : base(x => (string.IsNullOrEmpty(specParams.Search)
            || x.Headline.Contains(specParams.Search)
            || x.NewsTitle.Contains(specParams.Search)
            || x.CreatedBy.AccountName.Contains(specParams.Search))
            && (!specParams.CatId.HasValue 
            || x.CategoryId == specParams.CatId)
            && (!specParams.Status.HasValue || x.NewsStatus == specParams.Status)
            )
        {
            AddInclude(na => na.CreatedBy);
            AddInclude(na => na.Category);
            ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1),
                specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "dateAsc":
                        AddOrderBy(na => na.CreatedDate);
                        break;
                    case "dateDesc":
                        AddOrderByDescending(na => na.CreatedDate);
                        break;
                    case "titleDesc":
                        AddOrderByDescending(na => na.NewsTitle);
                        break;
                    case "titleAsc":
                        AddOrderBy(na => na.NewsTitle);
                        break;
                    default:
                        AddOrderByDescending(na => na.CreatedDate);
                        break;
                }
            }
        }

        public NewsArticleSpecification(int newsId) 
            : base(x => x.Id == newsId)
        {
            AddInclude(na => na.Tags);
            AddInclude(na => na.CreatedBy);
            AddInclude(na => na.Category);
        }

        public NewsArticleSpecification(int? accountId, DateTime? startDate, DateTime? endDate) : base(x =>
            (!accountId.HasValue || x.CreatedById == accountId)
            && (!(startDate.HasValue && endDate.HasValue)
            || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
        )
        {
            AddOrderByDescending(na => na.ModifiedDate);
            AddInclude(na => na.CreatedBy);
            AddInclude(na => na.Category);
        }

        public NewsArticleSpecification(bool? status, DateTime? startDate, DateTime? endDate) : base(x =>
            (!status.HasValue || x.NewsStatus == status)
            && (!(startDate.HasValue && endDate.HasValue)
            || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
        )
        {
        }
    }
}
