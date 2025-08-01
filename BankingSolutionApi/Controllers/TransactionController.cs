using AutoMapper;
using BankingSolutionApi.DTOs;
using BankingSolutionApi.Responses;
using BankingSolutionApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolutionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Validation failed"));

            var result = await _transactionService.DepositAsync(dto.AccountId, dto.Amount);
            return Ok(ApiResponse<TransactionResponseDto>.Ok(_mapper.Map<TransactionResponseDto>(result), "Deposit successful"));
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Validation failed"));

            var result = await _transactionService.WithdrawAsync(dto.AccountId, dto.Amount);
            return Ok(ApiResponse<TransactionResponseDto>.Ok(_mapper.Map<TransactionResponseDto>(result), "Withdraw successful"));
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Validation failed"));

            var (withdrawTx, depositTx) = await _transactionService.TransferAsync(dto.FromAccountId, dto.ToAccountId, dto.Amount);

            var dtoResult = new TransferResponseDto
            {
                Withdraw = _mapper.Map<TransactionResponseDto>(withdrawTx),
                Deposit = _mapper.Map<TransactionResponseDto>(depositTx)
            };

            return Ok(ApiResponse<TransferResponseDto>.Ok(dtoResult, "Transfer successful"));
        }
    }
}
