using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Categories
{
    public class CategoryCountSpecification : BaseSpecification<Category>
    {
        public CategoryCountSpecification(CategorySpecParams specParams) : base(x =>
            (string.IsNullOrEmpty(specParams.Search) 
            || x.CategoryName.Contains(specParams.Search))
            && (!specParams.CatId.HasValue 
            || x.ParentCategoryId == specParams.CatId)
            && (!specParams.Status.HasValue 
            || x.IsActive == specParams.Status)
        )
        {
        }
    }
}
