namespace BankingSolutionApi.DTOs
{
    public class AccountResponseDto
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public decimal Balance { get; set; }
    }
}
