using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Tags
{
    public class TagSpecification : BaseSpecification<Tag>
    {
        public TagSpecification(string? search) : base(x => 
            string.IsNullOrEmpty(search) 
            || x.TagName.Contains(search)
            || x.Note.Contains(search)
        )
        {
        }
    }
}
