using AutoMapper;
using BankingSolutionApi.DTOs;
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
                return BadRequest(ModelState);

            var account = await _accountService.CreateAccountAsync(dto.OwnerName, dto.InitialBalance);
            var response = _mapper.Map<AccountResponseDto>(account);

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound($"Account with ID {id} not found.");

            return Ok(_mapper.Map<AccountResponseDto>(account));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(_mapper.Map<IEnumerable<AccountResponseDto>>(accounts));
        }
    }
}
