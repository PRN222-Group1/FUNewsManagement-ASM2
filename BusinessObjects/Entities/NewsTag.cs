using System.Text.RegularExpressions;

namespace BusinessObjects.Entities
{
    public class NewsTag
    {
        public required int NewsArticleId { get; set; }
        public NewsArticle NewsArticle { get; set; }
        public required int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
