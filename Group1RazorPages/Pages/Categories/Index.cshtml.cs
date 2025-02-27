using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using DataAccessLayer.Specifications.Categories;
using Group1RazorPages.Extensions;
using Group1RazorPages.Helpers;
using Group1RazorPages.Interfaces;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Group1RazorPages.Pages.Categories
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly ISignalRService _signalRService;

        public IndexModel(ICategoryService categoryService,
            ISignalRService signalRService)
        {
            _categoryService = categoryService;
            _signalRService = signalRService;
        }

        public Pagination<CategoryDTO> Categories { get; set; }

        public FilterViewModel FilterModel { get; set; }

        [BindProperty(SupportsGet = true)]

        public CategorySpecParams SpecParams { get; set; } = new CategorySpecParams();

        //========================
        //    MODAL PROPERTIES
        //========================

        [BindProperty]
        public CategoryToAddOrUpdateDTO Category { get; set; } = new CategoryToAddOrUpdateDTO();

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [TempData]
        public bool ShowModal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitializeCategoriesAndFiltersAsync();

            return Page();
        }

        // Return Create Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowCreateModal()
        {
            await InitializeDropdownsAsync();
            ViewData["Action"] = "Create";

            return new PartialViewResult
            {
                ViewName = "_CategoryCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Return Edit Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowEditModal(int id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null) return NotFound();

            Category = new CategoryToAddOrUpdateDTO
            {
                CategoryId = existingCategory.ParentCategoryId.Value,
                CategoryName = existingCategory.CategoryName,
                CategoryDescription = existingCategory.CategoryDescription,
                Status = existingCategory.IsActive
            };

            ViewData["Action"] = "Edit";
            CategoryId = id;
            ViewData["SelectedCategoryId"] = Category.CategoryId;

            await InitializeDropdownsAsync();

            return new PartialViewResult
            {
                ViewName = "_CategoryCreateOrEditForm",
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
                await InitializeCategoriesAndFiltersAsync();
                return Page();
            }

            Category.Status = true;

            var result = await _categoryService.CreateCategoryAsync(Category);
            if (!result)
            {
                TempData["ErrorMessage"] = "Error creating Category!";
                return Page();
            }

            await InitializeCategoriesAndFiltersAsync();

            // Send SignalR Message to clients in order to load categories page
            await _signalRService.LoadCategories();

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
                await InitializeCategoriesAndFiltersAsync();
                return Page();
            }

            if (!CategoryId.HasValue)
            {
                TempData["ErrorMessage"] = "Category ID is missing!";
                return Page();
            }

            var result = await _categoryService.EditCategoryAsync(CategoryId.Value, Category);
            if (!result)
            {
                TempData["ErrorMessage"] = "Error updating Category!";
                return Page();
            }
            await InitializeCategoriesAndFiltersAsync();

            // Send SignalR Message to clients in order to load categories page
            await _signalRService.LoadCategories();

            return Page();
        }

        // Delete Category
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            await InitializeCategoriesAndFiltersAsync();

            if (!result)
            {
                TempData["ErrorMessage"] = "This category still have news articles!";
                return Page();
            }

            // Send SignalR Message to clients in order to load categories page
            await _signalRService.LoadCategories();

            return Page();
        }

        // Initialization
        private async Task InitializeCategoriesAndFiltersAsync()
        {
            await InitializeDropdownsAsync();

            var categories = await _categoryService.GetCategoriesAsync(SpecParams);
            var count = await _categoryService.CountCategoriesAsync(SpecParams);

            Categories = new Pagination<CategoryDTO>(
                SpecParams.PageNumber,
                SpecParams.PageSize,
                count,
                categories
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
                SelectedCategory = SpecParams.CatId,
                Categories = ViewData["Categories"] as IReadOnlyList<CategoryDTO>
            };

            ViewData["Action"] = "Create";
        }

        // Initialize Dropdowns
        private async Task InitializeDropdownsAsync()
        {
            var role = User.GetUserRole();

            SpecParams.Status = null;

            var categories = await _categoryService.GetCategoriesAsync(SpecParams);

            var cleanSpec = new CategorySpecParams();
            cleanSpec.Status = null;

            var parentCategories = await _categoryService.GetCategoriesAsync(cleanSpec);

            ViewData["Categories"] = parentCategories;
        }
    }
}
