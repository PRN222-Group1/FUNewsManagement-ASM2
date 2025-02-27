using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Categories
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(CategorySpecParams specParams) : base(x => 
            (string.IsNullOrEmpty(specParams.Search) || x.CategoryName.Contains(specParams.Search))
            && (!specParams.CatId.HasValue || x.ParentCategoryId == specParams.CatId)
            && (!specParams.Status.HasValue || x.IsActive == specParams.Status)
        )
        {
            AddInclude(c => c.ParentCategory);
            ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1),
                specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(c => c.CategoryName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(c => c.CategoryName);
                        break;
                    default:
                        AddOrderBy(c => c.CategoryName);
                        break;
                }
            }
        }

        public CategorySpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(c => c.NewsArticles);
        }
    }
}
