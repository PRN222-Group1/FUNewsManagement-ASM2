using System.ComponentModel.DataAnnotations;

namespace BusinessServiceLayer.DTOs
{
    public class SystemAccountToAddOrUpdateDTO
    {
        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int? AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%^&+=!]).{8,}$",
            ErrorMessage = "Password must contain at least one letter, one number, and one special character (@#$%^&+=!)")]
        public string AccountPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("AccountPassword", ErrorMessage = "Passwords do not match")]
        public string AccountConfirmPassword { get; set; }
    }
}
