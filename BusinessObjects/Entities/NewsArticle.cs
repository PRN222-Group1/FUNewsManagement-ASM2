namespace BusinessObjects.Entities;

public partial class NewsArticle : BaseEntity
{
    public string? NewsTitle { get; set; }

    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public string? ImageUrl { get; set; }

    public string? NewsSource { get; set; }

    public int? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
