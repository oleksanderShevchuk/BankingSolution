using AutoMapper;
using BankingSolutionApi.Data;
using BankingSolutionApi.Models;
using BankingSolutionApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSolutionApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;

        public AccountService(AppDbContext context, ILogger<AccountService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
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

            _logger.LogInformation("Account {AccountId} created for {OwnerName} with balance {Balance}",
                account.Id, ownerName, initialBalance);

            return account;
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                _logger.LogWarning("Account {AccountId} not found", id);

            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var accounts = await _context.Accounts.AsNoTracking().ToListAsync();
            _logger.LogInformation("Retrieved {Count} accounts", accounts.Count);
            return accounts;
        }
    }
}
