using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Group1RazorPages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group1RazorPages.Pages.NewsArticles
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DetailsModel(INewsArticleService newsArticleService) 
        {
            _newsArticleService = newsArticleService;
        }

        public NewsArticleDTO NewsArticle { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            NewsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (NewsArticle == null) return NotFound();

            ViewData["Role"] = User.GetUserRole();

            return Page();
        }
    }
}
