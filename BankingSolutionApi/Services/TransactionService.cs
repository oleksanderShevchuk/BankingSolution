using BankingSolutionApi.Data;
using BankingSolutionApi.Models;
using BankingSolutionApi.Services.Interfaces;

namespace BankingSolutionApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(AppDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Transaction> DepositAsync(int accountId, decimal amount)
        {
            var account = await GetAccountOrThrow(accountId);

            account.Balance += amount;
            var tx = CreateTransaction(accountId, amount, "Deposit");

            await SaveTransactionAsync(account, tx);
            _logger.LogInformation("Deposit {Amount} to Account {Id}", amount, accountId);

            return tx;
        }

        public async Task<Transaction> WithdrawAsync(int accountId, decimal amount)
        {
            var account = await GetAccountOrThrow(accountId);
            EnsureSufficientFunds(account, amount);

            account.Balance -= amount;
            var tx = CreateTransaction(accountId, amount, "Withdraw");

            await SaveTransactionAsync(account, tx);
            _logger.LogInformation("Withdraw {Amount} from Account {Id}", amount, accountId);

            return tx;
        }

        public async Task<(Transaction withdrawTx, Transaction depositTx)> TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            if (fromAccountId == toAccountId)
                throw new ArgumentException("Cannot transfer to the same account.");

            using var dbTx = await _context.Database.BeginTransactionAsync();

            var fromAccount = await GetAccountOrThrow(fromAccountId);
            var toAccount = await GetAccountOrThrow(toAccountId);

            EnsureSufficientFunds(fromAccount, amount);

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var withdrawTx = CreateTransaction(fromAccountId, amount, "TransferOut");
            var depositTx = CreateTransaction(toAccountId, amount, "TransferIn");

            _context.Transactions.AddRange(withdrawTx, depositTx);
            await _context.SaveChangesAsync();
            await dbTx.CommitAsync();

            _logger.LogInformation("Transferred {Amount} from Account {From} to {To}", amount, fromAccountId, toAccountId);

            return (withdrawTx, depositTx);
        }

        private async Task<Account> GetAccountOrThrow(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                throw new ArgumentException($"Account with ID {accountId} not found.");
            return account;
        }

        private static void EnsureSufficientFunds(Account account, decimal amount)
        {
            if (account.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");
        }

        private static Transaction CreateTransaction(int accountId, decimal amount, string type) =>
            new()
            {
                AccountId = accountId,
                Amount = amount,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };

        private async Task SaveTransactionAsync(Account account, Transaction tx)
        {
            _context.Accounts.Update(account);
            _context.Transactions.Add(tx);
            await _context.SaveChangesAsync();
        }
    }

}
