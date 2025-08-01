using BankingSolutionApi.DTOs;
using FluentValidation;

namespace BankingSolutionApi.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Owner name is required")
                .MaximumLength(100);

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Initial balance cannot be negative");
        }
    }
}
