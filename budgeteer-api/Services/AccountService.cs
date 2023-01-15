using BudgeteerAPI.Database;
using BudgeteerAPI.Models;
using BudgeteerAPI.Repositories;
using System.Transactions;

namespace BudgeteerAPI.Services
{
    public class AccountService
    {
        private readonly AccountRepository accountRepository;
        public AccountService(BudgeteerDbContext context)
        {
            this.accountRepository = new AccountRepository(context);
        }

        public async Task<Account?> GetAccountAsync(Account account)
        {
            return await accountRepository.GetAccountAsync(account.Id);
        }

        public async Task<Account?> GetAccountAsync(string accountId)
        {
            return await accountRepository.GetAccountAsync(accountId);
        }
        public async Task<List<Account>> GetUserAccountsAsync(User user)
        {
            return await accountRepository.GetUserAccountsAsync(user.Id);
        }


        public async Task<List<Account>> GetUserAccountsAsync(string userId)
        {
            return await accountRepository.GetUserAccountsAsync(userId);
        }

        public async Task<Account> AddAccountAsync(User user, Account account)
        {
            account.Id = Guid.NewGuid().ToString();
            account.UserId = user.Id;
            await accountRepository.InsertAccountAsync(account);
            return account;
        }

        public async Task UpdateAccountAsync(User user, Account account)
        {
            await accountRepository.UpdateAccountAsync(user, account);
        }

        public async Task DeleteAccountAsync(User user, string accountId)
        {
            await accountRepository.DeleteAccountAsync(user, accountId);
        }
    }
}
