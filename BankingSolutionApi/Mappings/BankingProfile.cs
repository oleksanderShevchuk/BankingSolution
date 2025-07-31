using AutoMapper;
using BankingSolutionApi.DTOs;
using BankingSolutionApi.Models;

namespace BankingSolutionApi.Mappings
{
    public class BankingProfile : Profile
    {
        public BankingProfile()
        {
            CreateMap<CreateAccountDto, Account>()
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance));

            CreateMap<Account, AccountResponseDto>();
            CreateMap<Transaction, TransactionResponseDto>();
        }
    }
}
