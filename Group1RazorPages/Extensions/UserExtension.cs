using System.Security.Claims;

namespace Group1RazorPages.Extensions
{
    public static class UserExtension
    {
        // Helper to get the current user ID
        public static int? GetCurrentUserId(this ClaimsPrincipal user)
        {
            var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return null;
            return int.Parse(userId);
        }

        // Helper to get the current user's role
        public static string? GetUserRole(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }
    }
}
