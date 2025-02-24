using BusinessObjects.Entities;

namespace BusinessServiceLayer.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
