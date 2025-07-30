using BankingSolutionApi.Data;
using BankingSolutionApi.Models;
using BankingSolutionApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSolutionApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccountAsync(string ownerName, decimal initialBalance)
        {
            var account = new Account
            {
                OwnerName = ownerName,
                Balance = initialBalance
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.AsNoTracking().ToListAsync();
        }
    }
}
