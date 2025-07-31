namespace BankingSolutionApi.DTOs
{
    public class TransferResponseDto
    {
        public TransactionResponseDto Withdraw { get; set; }
        public TransactionResponseDto Deposit { get; set; }
    }
}
