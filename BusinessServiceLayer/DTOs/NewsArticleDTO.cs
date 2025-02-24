using BusinessObjects.Entities;

namespace BusinessServiceLayer.DTOs
{
    public class NewsArticleDTO
    {
        public int Id { get; set; }
        public string NewsTitle { get; set; } = null!;
        public string Headline { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? NewsSource { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool NewsStatus { get; set; }
        public virtual ICollection<TagDTO>? Tags { get; set; }

    }
}
