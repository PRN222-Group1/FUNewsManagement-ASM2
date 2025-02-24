using System.ComponentModel.DataAnnotations;

namespace BusinessServiceLayer.DTOs
{
    public class CategoryToAddOrUpdateDTO
    {
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Category description is required")]
        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters")]
        public string CategoryDescription { get; set; }

        public bool Status { get; set; }
    }
}
