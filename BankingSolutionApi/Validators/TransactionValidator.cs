using BankingSolutionApi.DTOs;
using FluentValidation;

namespace BankingSolutionApi.Validators
{
    public class TransactionValidator : AbstractValidator<TransactionDto>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class TransferValidator : AbstractValidator<TransferDto>
    {
        public TransferValidator()
        {
            RuleFor(x => x.FromAccountId).GreaterThan(0);
            RuleFor(x => x.ToAccountId).GreaterThan(0)
                .NotEqual(x => x.FromAccountId).WithMessage("Cannot transfer to the same account");
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
