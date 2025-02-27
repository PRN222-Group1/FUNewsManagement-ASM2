using BusinessObjects.Enums;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using DataAccessLayer.Specifications.Account;
using Group1RazorPages.Helpers;
using Group1RazorPages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Group1RazorPages.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public Pagination<SystemAccountDTO> Accounts { get; set; }

        public FilterViewModel FilterModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public AccountSpecParams SpecParams { get; set; } = new AccountSpecParams();

        //========================
        //    MODAL PROPERTIES
        //========================

        [BindProperty]
        public SystemAccountToAddOrUpdateDTO Account { get; set; } = new SystemAccountToAddOrUpdateDTO();

        [BindProperty(SupportsGet = true)]
        public int? AccountId { get; set; }

        [TempData]
        public bool ShowModal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitializeAccountsAndFiltersAsync();

            return Page();
        }

        // Return Create Modal when AJAX called this handler
        public async Task<IActionResult> OnGetShowCreateModal()
        {
            ViewData["Action"] = "Create";
            InitializeDropdowns();

            return new PartialViewResult
            {
                ViewName = "_AccountCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
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
                AccountRole = role,
                Gender = gender,
                AccountPassword = existingAccount.AccountPassword,
                AccountConfirmPassword = existingAccount.AccountPassword
            };

            ViewData["Action"] = "Edit";
            AccountId = id;
            ViewData["SelectedRole"] = role;
            ViewData["SelectedGender"] = gender;

            return new PartialViewResult
            {
                ViewName = "_AccountCreateOrEditForm",
                ViewData = new ViewDataDictionary<object>(ViewData)
            };
        }

        // Create Account
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ViewData["Action"] = "Create";
                ShowModal = true;
                await InitializeAccountsAndFiltersAsync();
                return Page();
            }

            var result = await _accountService.CreateAccountAsync(Account);
            if (result)
            {
                TempData["SuccessMessage"] = "Account Created Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error creating Account!";
            }

            await InitializeAccountsAndFiltersAsync();
            return Page();
        }

        // Edit Account
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                // Set show Modal to true in order to keep
                // the modal open and show validation error
                ViewData["Action"] = "Edit";
                ShowModal = true;
                await InitializeAccountsAndFiltersAsync();
                return Page();
            }

            if (!AccountId.HasValue)
            {
                TempData["ErrorMessage"] = "Account ID is missing!";
                return Page();
            }

            var result = await _accountService.UpdateAccountAsync(AccountId.Value, Account);
            if (result)
            {
                TempData["SuccessMessage"] = "Account Updated Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating Account!";
            }
            await InitializeAccountsAndFiltersAsync();

            return Page();
        }

        // Delete Category
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Account Deleted Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Account Deleted failed!";
            }
            await InitializeAccountsAndFiltersAsync();

            return Page();
        }

        // Initialization
        private async Task InitializeAccountsAndFiltersAsync()
        {
            var accounts = await _accountService.GetAccountsAsync(SpecParams);
            var count = await _accountService.CountAccountsAsync(SpecParams);

            Accounts = new Pagination<SystemAccountDTO>(
                SpecParams.PageNumber,
                SpecParams.PageSize,
                count,
                accounts
            );

            FilterModel = new FilterViewModel
            {
                SortOptions = new List<SortOption>
                {
                    new SortOption { Value = "nameAsc", Text = "Name (A-Z)" },
                    new SortOption { Value = "nameDesc", Text = "Name (Z-A)" }
                },

                RolesList = new List<RoleOption>() {
                    new RoleOption { Value = 1, Text = "Staff" },
                    new RoleOption { Value = 2, Text = "Lecturer" },
                    new RoleOption { Value = 3, Text = "InActive" },
                },

                SearchPlaceholder = "Search category name...",
                SearchQuery = SpecParams.Search,
                SelectedSortOption = SpecParams.Sort,
                PageNumber = SpecParams.PageNumber,
                PageSize = SpecParams.PageSize,
                PageCount = Convert.ToInt32(Math.Ceiling((decimal)count / SpecParams.PageSize)),
                SelectedRoleOption = SpecParams.Role
            };

            InitializeDropdowns();
            ViewData["Action"] = "Create";
        }

        // Initialize Dropdowns
        private void InitializeDropdowns()
        {
            var rolesList = new List<RoleOption>() {
                new RoleOption { Value = 1, Text = "Staff" },
                new RoleOption { Value = 2, Text = "Lecturer" },
                new RoleOption { Value = 3, Text = "InActive" },
            };

            var gendersList = new List<GenderOption>() {
                new GenderOption { Value = 0, Text = "Male" },
                new GenderOption { Value = 1, Text = "Female" },
            };

            ViewData["RolesList"] = rolesList;
            ViewData["GendersList"] = gendersList;
        }
    }
}
