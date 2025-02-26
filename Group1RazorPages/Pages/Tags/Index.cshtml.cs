using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using DataAccessLayer.Specifications.Categories;
using DataAccessLayer.Specifications.Tags;
using Group1RazorPages.Extensions;
using Group1RazorPages.Helpers;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Group1RazorPages.Pages.Tags
{
    public class IndexModel : PageModel
    {

        private readonly ITagService _tagService;

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public Pagination<TagDTO> Tags { get; set; }

        public FilterViewModel FilterModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public TagSpecParams SpecParams { get; set; } = new TagSpecParams();

        //========================
        //    MODAL PROPERTIES
        //========================

        [BindProperty]
        public TagToAddOrUpdateDTO Tag { get; set; } = new TagToAddOrUpdateDTO();

        [BindProperty(SupportsGet = true)]
        public int? TagId { get; set; }

        [TempData]
        public bool ShowModal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitializeTagsAndFiltersAsync();

            return Page();
        }

        // Return Create Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowCreateModal()
        {
            ViewData["Action"] = "Create";

            return new PartialViewResult
            {
                ViewName = "_TagCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Return Edit Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowEditModal(int id)
        {
            var existingTag = await _tagService.GetTagByIdAsync(id);
            if (existingTag == null) return NotFound();

            Tag = new TagToAddOrUpdateDTO
            {
                TagName = existingTag.TagName,
                Note = existingTag.Note
            };

            ViewData["Action"] = "Edit";
            TagId = id;

            return new PartialViewResult
            {
                ViewName = "_TagCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Create Category
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ViewData["Action"] = "Create";
                ShowModal = true;
                await InitializeTagsAndFiltersAsync();
                return Page();
            }

            var result = await _tagService.CreateTagAsync(Tag);
            if (result)
            {
                TempData["SuccessMessage"] = "Category Created Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error creating Category!";
            }

            await InitializeTagsAndFiltersAsync();
            return Page();
        }

        // Edit Category
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ViewData["Action"] = "Edit";
                ShowModal = true;
                await InitializeTagsAndFiltersAsync();
                return Page();
            }

            if (!TagId.HasValue)
            {
                TempData["ErrorMessage"] = "Category ID is missing!";
                return Page();
            }

            var result = await _tagService.UpdateTagAsync(TagId.Value, Tag);
            if (result)
            {
                TempData["SuccessMessage"] = "Category Updated Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating Category!";
            }
            await InitializeTagsAndFiltersAsync();

            return Page();
        }

        // Initialization
        private async Task InitializeTagsAndFiltersAsync()
        {
            var tags = await _tagService.GetTagsAsync(SpecParams);
            var count = await _tagService.CountTagsAsync(SpecParams);

            Tags = new Pagination<TagDTO>(
                SpecParams.PageNumber,
                SpecParams.PageSize,
                count,
                tags
            );

            FilterModel = new FilterViewModel
            {
                SortOptions = new List<SortOption>
            {
                new SortOption { Value = "nameAsc", Text = "Name (A-Z)" },
                new SortOption { Value = "nameDesc", Text = "Name (Z-A)" }
            },
                SearchPlaceholder = "Search category name...",
                SearchQuery = SpecParams.Search,
                SelectedSortOption = SpecParams.Sort,
                PageNumber = SpecParams.PageNumber,
                PageSize = SpecParams.PageSize,
                PageCount = Convert.ToInt32(Math.Ceiling((decimal)count / SpecParams.PageSize)),
            };

            ViewData["Action"] = "Create";
        }
    }
        
}
