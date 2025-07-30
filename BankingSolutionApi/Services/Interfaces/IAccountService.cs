using BankingSolutionApi.Models;

namespace BankingSolutionApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(string ownerName, decimal initialBalance);
        Task<Account?> GetAccountByIdAsync(int id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
    }
}
