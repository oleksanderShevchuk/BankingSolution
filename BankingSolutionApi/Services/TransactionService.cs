using BankingSolutionApi.Data;
using BankingSolutionApi.Models;
using BankingSolutionApi.Services.Interfaces;

namespace BankingSolutionApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> DepositAsync(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                throw new ArgumentException($"Account with ID {accountId} not found.");

            account.Balance += amount;

            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = "Deposit"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                throw new ArgumentException($"Account with ID {accountId} not found.");

            if (account.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            account.Balance -= amount;

            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = "Withdraw"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<(Transaction withdrawTx, Transaction depositTx)> TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            if (fromAccountId == toAccountId)
                throw new ArgumentException("Cannot transfer to the same account.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts.FindAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("One or both accounts not found.");

            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            // Update balances
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            // Create transaction records
            var withdrawTx = new Transaction
            {
                AccountId = fromAccountId,
                Amount = amount,
                Type = "TransferOut"
            };
            var depositTx = new Transaction
            {
                AccountId = toAccountId,
                Amount = amount,
                Type = "TransferIn"
            };

            _context.Transactions.AddRange(withdrawTx, depositTx);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (withdrawTx, depositTx);
        }
    }
}
