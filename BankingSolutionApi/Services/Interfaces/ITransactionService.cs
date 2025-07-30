using BankingSolutionApi.Models;

namespace BankingSolutionApi.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> DepositAsync(int accountId, decimal amount);
        Task<Transaction> WithdrawAsync(int accountId, decimal amount);
        Task<(Transaction withdrawTx, Transaction depositTx)> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
    }
}
