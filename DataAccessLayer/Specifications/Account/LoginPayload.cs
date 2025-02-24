using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Specifications.Account
{
    public class LoginPayload
    {
        [EmailAddress]
        public required string AccountEmail { get; set; }
        public required string AccountPassword { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
