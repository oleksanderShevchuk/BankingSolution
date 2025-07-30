using System.ComponentModel.DataAnnotations;

namespace BankingSolutionApi.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; } = 0;
    }
}
