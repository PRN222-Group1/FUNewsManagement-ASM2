using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Tags
{
    public class TagSpecification : BaseSpecification<Tag>
    {
        public TagSpecification(TagSpecParams specParams) : base(x => 
            string.IsNullOrEmpty(specParams.Search) 
            || x.TagName.Contains(specParams.Search)
            || x.Note.Contains(specParams.Search)
        )
        {
            ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1),
                specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(t => t.TagName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(t => t.TagName);
                        break;
                    default:
                        AddOrderBy(t => t.TagName);
                        break;
                }
            }
        }
    }
}
