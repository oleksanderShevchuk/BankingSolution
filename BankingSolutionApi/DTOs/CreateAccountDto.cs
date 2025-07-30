using System.ComponentModel.DataAnnotations;

namespace BankingSolutionApi.DTOs
{
    public class CreateAccountDto
    {
        [Required]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal InitialBalance { get; set; } = 0;
    }
}
