using System.ComponentModel.DataAnnotations;

namespace BankingSolutionApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } // Deposit, Withdraw, Transfer

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
