using Group1RazorPages.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Group1RazorPages.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<SignalRServer> _hubContext;

        public SignalRService(IHubContext<SignalRServer> hubContext) 
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Reload /NewsArticles/Index page
        /// </summary>
        /// <returns></returns>
        public async Task LoadNewsArticles()
        {
            await _hubContext.Clients.All.SendAsync("LoadNewsArticles");
        }

        /// <summary>
        /// Reload /NewsArticles/Details/id page
        /// </summary>
        /// <returns></returns>
        public async Task LoadNewsArticleDetails(int id)
        {
            await _hubContext.Clients.All.SendAsync("LoadNewsArticleDetails", id);
        }

        /// <summary>
        /// Reload /Accounts/Index page
        /// </summary>
        /// <returns></returns>
        public async Task LoadAccounts()
        {
            await _hubContext.Clients.All.SendAsync("LoadAccounts");
        }

        /// <summary>
        /// Reload /Accounts/Details/id page
        /// </summary>
        /// <returns></returns>
        public async Task LoadProfile(int id)
        {
            await _hubContext.Clients.All.SendAsync("LoadProfile", id);
        }

        /// <summary>
        /// Reload /Categories/Index page
        /// </summary>
        /// <returns></returns>
        public async Task LoadCategories()
        {
            await _hubContext.Clients.All.SendAsync("LoadCategories");
        }

        /// <summary>
        /// Reload /NewsArticles/IndexHistory page
        /// </summary>
        /// <returns></returns>
        public async Task LoadHistory(int id)
        {
            await _hubContext.Clients.All.SendAsync("LoadHistory", id);
        }

        /// <summary>
        /// Reload /Reports/Index page
        /// </summary>
        /// <returns></returns>
        public async Task LoadReports()
        {
            await _hubContext.Clients.All.SendAsync("LoadReports");
        }

        /// <summary>
        /// Reload /Tags/Index page
        /// </summary>
        /// <returns></returns>
        public async Task LoadTags()
        {
            await _hubContext.Clients.All.SendAsync("LoadTags");
        }
    }
}
