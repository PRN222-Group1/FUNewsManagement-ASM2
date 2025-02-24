using BusinessObjects.Enums;

namespace BusinessObjects.Entities;

public partial class SystemAccount : BaseEntity
{
    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public Gender? Gender { get; set; }

    public string? ImageUrl { get; set; }

    public int? AccountRole { get; set; }

    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
