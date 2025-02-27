using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Group1RazorPages.Extensions;
using Group1RazorPages.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group1RazorPages.Pages.NewsArticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IUploadService _uploadService;
        private readonly ISignalRService _signalRService;

        public DetailsModel(INewsArticleService newsArticleService, 
            IUploadService uploadService, ISignalRService signalRService) 
        {
            _newsArticleService = newsArticleService;
            _uploadService = uploadService;
            _signalRService = signalRService;
        }

        public NewsArticleDTO NewsArticleDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await InitializeNewsArticleAsync(id);

            return Page();
        }

        // Handle Profile Image Upload
        public async Task<IActionResult> OnPostUploadImageAsync(int id, IFormFile newsArticleImage)
        {
            // Use the upload service
            var uploadResult = await _uploadService.UploadFileAsync(
                newsArticleImage,
                "articles",
                id.ToString()
            );

            if (!uploadResult.Success)
            {
                TempData["ErrorMessage"] = uploadResult.ErrorMessage;
                await InitializeNewsArticleAsync(id);
                return Page();
            }

            var currentUserId = User.GetCurrentUserId().Value;

            // Update account with the new image URL
            var result = await _newsArticleService.UpdateNewsArticleImageAsync(id, currentUserId, uploadResult.RelativeUrl);
            await InitializeNewsArticleAsync(id);

            if (!result)
            {
                await _uploadService.DeleteFileAsync(uploadResult.RelativeUrl);
                TempData["ErrorMessage"] = "Error updating profile image.";
                return Page();
            }

            // Send SignalR Message to clients in order to load news articles page
            await _signalRService.LoadNewsArticles();

            // Send SignalR Message to clients in order to load news article details page
            await _signalRService.LoadNewsArticleDetails(id);

            return Page();
        }

        private async Task InitializeNewsArticleAsync(int id)
        {
            NewsArticleDetails = await _newsArticleService.GetNewsArticleByIdAsync(id);
            ViewData["Role"] = User.GetUserRole();
        }
    }
}
