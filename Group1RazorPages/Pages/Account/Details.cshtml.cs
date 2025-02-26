using BusinessObjects.Enums;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Group1RazorPages.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DetailsModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public SystemAccountDTO AccountDetails { get; set; }

        //========================
        //    MODAL PROPERTIES
        //========================

        [BindProperty]
        public SystemAccountToAddOrUpdateDTO Account { get; set; } = new SystemAccountToAddOrUpdateDTO();

        [BindProperty(SupportsGet = true)]
        public int? AccountId { get; set; }

        [TempData]
        public bool ShowModal { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await InitializeAccountAsync(id);
            return Page();
        }

        // Return Edit Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowEditModal(int id)
        {
            InitializeDropdowns();

            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null) return NotFound();

            // Convert account role from string to enum value
            var role = (int)Enum.Parse(typeof(Role), existingAccount.AccountRole);

            Account = new SystemAccountToAddOrUpdateDTO
            {
                AccountName = existingAccount.AccountName,
                AccountEmail = existingAccount.AccountEmail,
                AccountRole = role
            };

            AccountId = id;

            return new PartialViewResult
            {
                ViewName = "_ProfileEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Edit Account
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ShowModal = true;
                await InitializeAccountAsync(AccountId.Value);
                return Page();
            }

            var result = await _accountService.UpdateAccountAsync(AccountId.Value, Account);
            if (result)
            {
                TempData["SuccessMessage"] = "Profile Updated Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating Profile!";
            }
            await InitializeAccountAsync(AccountId.Value);

            return Page();
        }

        private async Task InitializeAccountAsync(int id)
        {
            AccountDetails = await _accountService.GetAccountByIdAsync(id);

            InitializeDropdowns();
        }

        // Initialize Dropdowns
        private void InitializeDropdowns()
        {
            var rolesList = new List<RoleOption>();

            ViewData["RolesList"] = rolesList;
        }
    }
}
