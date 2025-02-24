using System.ComponentModel.DataAnnotations;

namespace BusinessServiceLayer.DTOs
{
    public class NewsArticleToAddOrUpdateDTO
    {
        [Required(ErrorMessage = "News title is required")]
        [StringLength(150, ErrorMessage = "News title cannot exceed 150 characters")]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(250, ErrorMessage = "Headline cannot exceed 250 characters")]
        public string Headline { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "News content is required")]
        public string NewsContent { get; set; }

        [StringLength(200, ErrorMessage = "News source cannot exceed 200 characters")]
        public string? NewsSource { get; set; }

        public int? CategoryId { get; set; }

        public bool NewsStatus { get; set; } = true;

        public int? CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

        public string? TagIds { get; set; }
    }
}
