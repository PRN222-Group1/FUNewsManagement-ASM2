namespace BusinessServiceLayer.DTOs
{
    public class ReportDTO
    {
        public IReadOnlyList<Dictionary<string, int>> ArticlesCreatedByAuthor;
        public int PublishedArticles { get; set; }
        public int DraftArticles { get; set; }
        public int TotalArticles { get; set; }
    }
}
