using BusinessServiceLayer.DTOs;

namespace Group1RazorPages.ViewModels
{
    public class SortOption
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class RoleOption
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class FilterViewModel
    {
        public string SearchPlaceholder { get; set; }
        public string SearchQuery { get; set; }
        public string SelectedSortOption { get; set; }
        public int? SelectedRoleOption { get; set; }
        public int? SelectedCategory { get; set; } = null;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<SortOption> SortOptions { get; set; }
        public IReadOnlyList<CategoryDTO> Categories { get; set; }
        public List<RoleOption> RolesList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
