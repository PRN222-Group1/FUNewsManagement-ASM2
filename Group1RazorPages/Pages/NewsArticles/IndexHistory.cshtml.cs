using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group1RazorPages.Pages.NewsArticles
{
    [Authorize(Roles = "Staff")]
    public class IndexHistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public IndexHistoryModel(INewsArticleService newsArticleService) 
        {
            _newsArticleService = newsArticleService;
        }

        public IReadOnlyList<NewsArticleDTO> NewsArticles { get; set; }

        public FilterViewModel FilterModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; } = null;

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(int id, DateTime? startDate, DateTime? endDate)
        {
            // Initialize properties
            FilterModel = new FilterViewModel()
            {
                StartDate = StartDate,
                EndDate = EndDate,
            };

            NewsArticles = await _newsArticleService.GetNewsArticleHistoryAsync(id, startDate, endDate);

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate.Value > endDate.Value)
                {
                    TempData["ErrorMessage"] = "Start Date cannot be older than End Date";
                    return Page();
                }
            }

            return Page();
        }
    }
}
