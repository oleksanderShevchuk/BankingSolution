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

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _accountService.CreateAccountAsync(dto.OwnerName, dto.InitialBalance);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound($"Account with ID {id} not found.");

            return Ok(account);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }
    }
}
