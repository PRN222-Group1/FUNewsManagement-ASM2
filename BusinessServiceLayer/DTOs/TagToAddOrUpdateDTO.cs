using System.ComponentModel.DataAnnotations;

namespace BusinessServiceLayer.DTOs
{
    public class TagToAddOrUpdateDTO
    {
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(20, ErrorMessage = "Tag name cannot exceed 20 characters")]
        public string TagName { get; set; }

        [Required(ErrorMessage = "Tag note is required")]
        [StringLength(200, ErrorMessage = "Tag note cannot exceed 200 characters")]
        public string Note { get; set; }
    }
}
