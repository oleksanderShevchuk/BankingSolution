using AutoMapper;
using BankingSolutionApi.DTOs;
using BankingSolutionApi.Responses;
using BankingSolutionApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolutionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Validation failed"));

            var account = await _accountService.CreateAccountAsync(dto.OwnerName, dto.InitialBalance);
            var response = _mapper.Map<AccountResponseDto>(account);

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, ApiResponse<AccountResponseDto>.Ok(response));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound(ApiResponse<string>.Fail($"Account with ID {id} not found."));

            var dto = _mapper.Map<AccountResponseDto>(account);
            return Ok(ApiResponse<AccountResponseDto>.Ok(dto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts([FromQuery] AccountQueryDto query)
        {
            var (accounts, totalCount) = await _accountService.GetAccountsAsync(query.OwnerName, query.Page, query.PageSize);

            var dto = new
            {
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize,
                Items = _mapper.Map<IEnumerable<AccountResponseDto>>(accounts)
            };

            return Ok(ApiResponse<object>.Ok(dto));
        }
    }
}
