using System.ComponentModel.DataAnnotations;

namespace BankingSolutionApi.DTOs
{
    public class TransactionDto
    {
        [Required]
        public int AccountId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }

    public class TransferDto
    {
        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
