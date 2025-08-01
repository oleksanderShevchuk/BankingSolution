namespace BankingSolutionApi.DTOs
{
    public class AccountQueryDto
    {
        public string? OwnerName { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
