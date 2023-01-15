using BudgeteerAPI.Database;
using BudgeteerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BudgeteerAPI.Repositories
{
    public class AccountRepository : AbstractRepository
    {
        public AccountRepository(BudgeteerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account?> GetAccountAsync(string accountId)
        {
            Account? dbAccount = await db.Accounts.FindAsync(accountId);
            return dbAccount == default ? null : dbAccount;
        }

        public async Task<List<Account>> GetUserAccountsAsync(string userId)
        {
            return await db.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task InsertAccountAsync(Account account)
        {
            await db.Accounts.AddAsync(account);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(User user, Account account)
        {
            Account? dbAccount = await db.Accounts.FindAsync(account.Id);
            if (dbAccount == null || dbAccount.UserId != user.Id)
            {
                throw new TransactionException($"Could not find account with ID '{account.Id}' for user '{user.Id}'");
            }

            dbAccount.Type = account.Type;
            dbAccount.Bank = account.Bank;
            dbAccount.Name = account.Name;
            await db.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(User user, string accountId)
        {
            Account? dbAccount = await db.Accounts.FindAsync(accountId);
            if (dbAccount == null || dbAccount.UserId != user.Id)
            {
                throw new TransactionException($"Could not find account with ID '{accountId}' for user '{user.Id}'");
            }

            db.Accounts.Remove(dbAccount);
            db.SaveChanges();
        }
    }
}
