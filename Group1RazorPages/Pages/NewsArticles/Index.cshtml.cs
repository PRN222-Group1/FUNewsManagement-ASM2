using BusinessObjects.Enums;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using DataAccessLayer.Specifications.NewsArticles;
using Group1RazorPages.Extensions;
using Group1RazorPages.Helpers;
using Group1RazorPages.Interfaces;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data;

namespace Group1RazorPages.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ISignalRService _signalRService;

        public IndexModel(INewsArticleService newsArticleService, 
            ISignalRService signalRService)
        {
            _newsArticleService = newsArticleService;
            _signalRService = signalRService;
        }

        public Pagination<NewsArticleDTO> NewsArticles { get; set; }

        public FilterViewModel FilterModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public NewsArticleSpecParams SpecParams { get; set; } = new NewsArticleSpecParams();

        //========================
        //    MODAL PROPERTIES
        //========================

        [BindProperty]
        public NewsArticleToAddOrUpdateDTO NewsArticle { get; set; } = new NewsArticleToAddOrUpdateDTO();

        [BindProperty(SupportsGet = true)]
        public int? ArticleId { get; set; }

        [TempData]
        public bool ShowModal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitializeNewsArticlesAndFiltersAsync();

            return Page();
        }

        // Return Create Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowCreateModal()
        {
            await InitializeDropdownsAsync();
            ViewData["Action"] = "Create";

            return new PartialViewResult
            {
                ViewName = "_NewsArticleCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Return Edit Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowEditModal(int id)
        {
            var existingArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (existingArticle == null) return NotFound();

            NewsArticle = new NewsArticleToAddOrUpdateDTO
            {
                Headline = existingArticle.Headline,
                NewsTitle = existingArticle.NewsTitle,
                NewsContent = existingArticle.NewsContent,
                CategoryId = existingArticle.CategoryId,
                NewsSource = existingArticle.NewsSource,
                NewsStatus = existingArticle.NewsStatus,
                TagIds = string.Join(",", existingArticle.Tags.Select(tag => tag.Id.ToString()))
            };

            ViewData["Action"] = "Edit";
            ArticleId = id;
            ViewData["SelectedCategoryId"] = NewsArticle.CategoryId;
            ViewData["SelectedTagIds"] = NewsArticle.TagIds;

            await InitializeDropdownsAsync();

            return new PartialViewResult
            {
                ViewName = "_NewsArticleCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Create News Article
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ViewData["Action"] = "Create";
                ShowModal = true;
                await InitializeNewsArticlesAndFiltersAsync();
                return Page();
            }

            var currentUserId = User.GetCurrentUserId();

            NewsArticle.CreatedDate = DateTime.UtcNow;
            NewsArticle.CreatedById = currentUserId;
            NewsArticle.UpdatedById = currentUserId;

            // Create a draft before publishing
            NewsArticle.NewsStatus = false;

            var result = await _newsArticleService.CreateNewsArticleAsync(NewsArticle);
            await InitializeNewsArticlesAndFiltersAsync();

            if (!result)
            {
                TempData["ErrorMessage"] = "Error creating News Article!";
                return Page();
            }

            // Send SignalR Message to clients in order to load news articles
            await _signalRService.LoadNewsArticles();

            // Send SignalR Message to clients in order to load reports
            await _signalRService.LoadReports();

            // Send SignalR Message to clients in order to load history
            await _signalRService.LoadHistory(currentUserId.Value);

            return Page();
        }

        // Edit News Article
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ViewData["Action"] = "Edit";
                ShowModal = true;
                await InitializeNewsArticlesAndFiltersAsync();
                return Page();
            }

            var currentUserId = User.GetCurrentUserId();

            if (!ArticleId.HasValue)
            {
                TempData["ErrorMessage"] = "Article ID is missing!";
                return Page();
            }

            NewsArticle.ModifiedDate = DateTime.UtcNow;
            NewsArticle.UpdatedById = currentUserId;

            var result = await _newsArticleService.UpdateNewsArticleAsync(ArticleId.Value, NewsArticle);
            await InitializeNewsArticlesAndFiltersAsync();

            if (!result)
            {
                TempData["ErrorMessage"] = "Error updating News Article!";
                return Page();
            }

            // Send SignalR Message to clients in order to load news articles
            await _signalRService.LoadNewsArticles();

            // Send SignalR Message to clients in order to load history
            await _signalRService.LoadHistory(currentUserId.Value);

            // Send SignalR Message to clients in order to load reports
            await _signalRService.LoadReports();

            // Send SignalR Message to clients in order to load news article details page
            await _signalRService.LoadNewsArticleDetails(ArticleId.Value);

            return Page();
        }

        // Delete News Article
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _newsArticleService.DeleteNewsArticleAsync(id);
            await InitializeNewsArticlesAndFiltersAsync();

            if (!result)
            {
                TempData["ErrorMessage"] = "Error deleting News Article!";
                return Page();
            }

            var currentUserId = User.GetCurrentUserId().Value;

            // Send SignalR Message to clients in order to load news articles page
            await _signalRService.LoadNewsArticles();

            // Send SignalR Message to clients in order to load reports
            await _signalRService.LoadReports();

            // Send SignalR Message to clients in order to load history
            await _signalRService.LoadHistory(currentUserId);

            return Page();
        }

        // Initialization
        private async Task InitializeNewsArticlesAndFiltersAsync()
        {
            await InitializeDropdownsAsync();

            var newsArticles = await _newsArticleService.GetNewsArticlesAsync(SpecParams);
            var count = await _newsArticleService.CountNewsArticlesAsync(SpecParams);

            NewsArticles = new Pagination<NewsArticleDTO>(
                SpecParams.PageNumber,
                SpecParams.PageSize,
                count,
                newsArticles
            );

            FilterModel = new FilterViewModel
            {
                SortOptions = new List<SortOption>
                {
                    new SortOption { Value = "dateDesc", Text = "Newest First" },
                    new SortOption { Value = "dateAsc", Text = "Oldest First" },
                    new SortOption { Value = "titleAsc", Text = "Title (A-Z)" },
                    new SortOption { Value = "titleDesc", Text = "Title (Z-A)" }
                },
                SearchPlaceholder = "Search articles, authors...",
                SearchQuery = SpecParams.Search,
                SelectedSortOption = SpecParams.Sort,
                PageNumber = SpecParams.PageNumber,
                PageSize = SpecParams.PageSize,
                PageCount = Convert.ToInt32(Math.Ceiling((decimal)count / SpecParams.PageSize)),
                SelectedCategory = SpecParams.CatId,
                Categories = ViewData["Categories"] as IReadOnlyList<CategoryDTO>
            };

            ViewData["Action"] = "Create";
        }

        // Initialize Dropdowns
        private async Task InitializeDropdownsAsync()
        {
            var role = User.GetUserRole();

            // Show all news for staff but only published news for lecturer
            SpecParams.Status = role == Role.Staff.ToString() ? null : true;

            var categories = await _newsArticleService.GetAllCategories();
            var tags = await _newsArticleService.GetAllTags();

            ViewData["Categories"] = categories;
            ViewData["Tags"] = tags;
            ViewData["Role"] = role;
        }
    }
}