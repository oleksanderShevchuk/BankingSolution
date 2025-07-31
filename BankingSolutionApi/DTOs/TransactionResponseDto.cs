namespace BankingSolutionApi.DTOs
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
