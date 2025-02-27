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
        private readonly IUploadService _uploadService;

        public DetailsModel(IAccountService accountService, IUploadService uploadService)
        {
            _accountService = accountService;
            _uploadService = uploadService;
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

            // Convert account role and gender from string to enum value
            var role = (int)Enum.Parse(typeof(Role), existingAccount.AccountRole);
            var gender = (int)Enum.Parse(typeof(Gender), existingAccount.Gender);

            Account = new SystemAccountToAddOrUpdateDTO
            {
                AccountName = existingAccount.AccountName,
                AccountEmail = existingAccount.AccountEmail,
                Gender = gender,
                AccountRole = role
            };

            AccountId = id;
            ViewData["SelectedGender"] = gender;

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

        // Handle Profile Image Upload
        public async Task<IActionResult> OnPostUploadImageAsync(int id, IFormFile profileImage)
        {
            // Use the upload service
            var uploadResult = await _uploadService.UploadFileAsync(
                profileImage,
                "profiles",
                id.ToString()
            );

            if (!uploadResult.Success)
            {
                TempData["ErrorMessage"] = uploadResult.ErrorMessage;
                await InitializeAccountAsync(id);
                return Page();
            }

            // Update account with the new image URL
            var result = await _accountService.UpdateAccountImageAsync(id, uploadResult.RelativeUrl);

            if (result)
            {
                TempData["SuccessMessage"] = "Profile image updated successfully!";
            }
            else
            {
                await _uploadService.DeleteFileAsync(uploadResult.RelativeUrl);
                TempData["ErrorMessage"] = "Error updating profile image.";
            }

            await InitializeAccountAsync(id);
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
            var gendersList = new List<GenderOption>() {
                new GenderOption { Value = 0, Text = "Male" },
                new GenderOption { Value = 1, Text = "Female" },
            };

            ViewData["GendersList"] = gendersList;
        }
    }
}
