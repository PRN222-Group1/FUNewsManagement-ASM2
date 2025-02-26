using BusinessObjects.Enums;
using BusinessServiceLayer.Interfaces;
using DataAccessLayer.Specifications.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Group1RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        public LoginModel(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _adminEmail = configuration["AdminAccount:Email"];
            _adminPassword = configuration["AdminAccount:Password"];
        }

        [BindProperty]
        public LoginPayload LoginPayload { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
               return RedirectToPage("/NewsArticles/Index");
            }
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var returnUrl = LoginPayload.ReturnUrl ?? "~/NewsArticles/Index";

            if (ModelState.IsValid)
            {
                ClaimsIdentity identity = null;
                bool isAuthenticated = false;

                if (LoginPayload.AccountEmail == _adminEmail
                    && LoginPayload.AccountPassword == _adminPassword)
                {
                    // Create the identity for the Admin
                    identity = CreateIdentity(_adminEmail,
                        Role.Admin.ToString(), -1);

                    isAuthenticated = true;
                }
                else
                {
                    // Get the non-admin user and sign in
                    var user = await _accountService.LoginAsync(LoginPayload);
                    if (user != null)
                    {
                        // Create the identity for the user
                        identity = CreateIdentity(user.AccountEmail,
                            user.AccountRole, user.Id.Value);

                        isAuthenticated = true;
                    }
                }

                // If is authenticated create a claims principal and sign in with that claims
                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return LocalRedirect(returnUrl);
                }
            }

            TempData["ErrorMessage"] = "Incorrect email or password or the user account is deleted!";
            return Page();
        }

        private ClaimsIdentity CreateIdentity(string email, string role, int userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}