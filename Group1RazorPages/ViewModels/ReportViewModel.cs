namespace Group1RazorPages.ViewModels
{
    public class ReportViewModel
    {
        public Dictionary<string, int> ArticlesCreatedByAuthor;
        public int PublishedArticles { get; set; }
        public int DraftArticles { get; set; }
        public int TotalArticles { get; set; }
    }
}
