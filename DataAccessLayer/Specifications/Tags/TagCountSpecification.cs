using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Tags
{
    public class TagCountSpecification : BaseSpecification<Tag>
    {
        public TagCountSpecification(TagSpecParams specParams) : base(x =>
            string.IsNullOrEmpty(specParams.Search)
            || x.TagName.Contains(specParams.Search)
            || x.Note.Contains(specParams.Search)
        )
        {
        }
    }
}
