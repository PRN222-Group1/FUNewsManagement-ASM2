namespace Group1RazorPages.Interfaces
{
    public interface ISignalRService
    {
        Task LoadNewsArticles();
        Task LoadNewsArticleDetails(int id);
        Task LoadAccounts();
        Task LoadProfile(int id);
        Task LoadCategories();
        Task LoadHistory(int id);
        Task LoadReports();
        Task LoadTags();
    }
}
