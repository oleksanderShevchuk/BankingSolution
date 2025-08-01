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

        public async Task<(IEnumerable<Account> Accounts, int TotalCount)> GetAccountsAsync(string? ownerName, int page, int pageSize)
        {
            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(ownerName))
                query = query.Where(a => a.OwnerName.Contains(ownerName));

            var totalCount = await query.CountAsync();

            var accounts = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (accounts, totalCount);
        }

    }
}
