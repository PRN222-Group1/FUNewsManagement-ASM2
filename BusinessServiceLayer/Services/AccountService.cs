using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Specifications.Account;

namespace BusinessServiceLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<SystemAccount> _accountService;
        private readonly IMapper _mapper;

        public AccountService(IGenericRepository<SystemAccount> accountService, IMapper mapper) 
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<SystemAccountDTO?> LoginAsync(LoginPayload loginPayload)
        {
            var spec = new AccountSpecification(loginPayload);
            var user = await _accountService.GetEntityWithSpec(spec);

            // If the user does not exist or is inactive then do not authorize
            if (user == null || user.AccountRole == (int) Role.Inactive)
            {
                return null;
            }

            return _mapper.Map<SystemAccount, SystemAccountDTO>(user);
        }

        public async Task<bool> CheckEmailExistedAsync(string email)
        {
            var spec = new AccountSpecification(email);
            var user = await _accountService.GetEntityWithSpec(spec);
            if (user != null) return true;
            return false;
        }

        public async Task<SystemAccountDTO?> GetUserByEmailAsync(string email)
        {
            var spec = new AccountSpecification(email);
            var user = await _accountService.GetEntityWithSpec(spec);

            if (user == null) 
            {
                return null;
            }

            return _mapper.Map<SystemAccount, SystemAccountDTO>(user);
        }

        public async Task<IReadOnlyList<SystemAccountDTO>> GetAccountsAsync(AccountSpecParams specParams)
        {
            var spec = new AccountSpecification(specParams);
            var accounts = await _accountService.ListAsync(spec);
            return _mapper.Map<IReadOnlyList<SystemAccount>, IReadOnlyList<SystemAccountDTO>>(accounts);
        }

        public async Task<int> CountAccountsAsync(AccountSpecParams specParams)
        {
            var spec = new AccountCountSpecification(specParams);
            var count = await _accountService.CountAsync(spec);
            return count;
        }

        public async Task<SystemAccountDTO> GetAccountByIdAsync(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            return _mapper.Map<SystemAccount, SystemAccountDTO>(account);
        }

        public async Task<bool> CreateAccountAsync(SystemAccountToAddOrUpdateDTO account)
        {
            var accountToAdd = _mapper.Map<SystemAccountToAddOrUpdateDTO, SystemAccount>(account);
            _accountService.Add(accountToAdd);
            var result = await _accountService.SaveAllAsync();
            return result;
        }

        public async Task<bool> UpdateAccountAsync(int id, SystemAccountToAddOrUpdateDTO account)
        {
            var result = false;
            var accountToUpdate = await _accountService.GetByIdAsync(id);

            if (accountToUpdate == null)
            {
                return result;
            }
            
            accountToUpdate.AccountName = account.AccountName;
            accountToUpdate.AccountEmail = account.AccountEmail;
            accountToUpdate.AccountRole = account.AccountRole;
            accountToUpdate.AccountPassword = account.AccountPassword;

            _accountService.Update(accountToUpdate);
            result = await _accountService.SaveAllAsync();
            return result;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var result = false;
            var accountToDelete = await _accountService.GetByIdAsync(id);

            if (accountToDelete == null)
            {
                return result;
            }

            accountToDelete.AccountRole = (int) Role.Inactive;

            _accountService.Update(accountToDelete);
            result = await _accountService.SaveAllAsync();
            return result;
        }
    }
}
