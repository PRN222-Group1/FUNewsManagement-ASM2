using BusinessServiceLayer.Interfaces;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group1RazorPages.Pages.Reports
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;

        public IndexModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public ReportViewModel ReportViewModel { get; set; }
        public FilterViewModel FilterModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; } = null;

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            // Initialize properties
            FilterModel = new FilterViewModel()
            {
                StartDate = StartDate,
                EndDate = EndDate,
            };

            // Get statistics
            var publishedArticles = await _reportService.CountPublishedNewsArticlesAsync(startDate, endDate);
            var draftArticles = await _reportService.CountDraftNewsArticlesAsync(startDate, endDate);
            var totalArticles = await _reportService.CountTotalNewsArticlesAsync(startDate, endDate);
            var dictList = await _reportService.GetArticlesCountByAuthorAsync(startDate, endDate);

            ReportViewModel = new ReportViewModel
            {
                PublishedArticles = publishedArticles,
                DraftArticles = draftArticles,
                TotalArticles = totalArticles,
                ArticlesCreatedByAuthor = dictList
            };

            // Validate date filter
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
