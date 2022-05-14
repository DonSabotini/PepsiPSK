using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Entities;
using PepsiPSK.Models.Transaction;
using PepsiPSK.Services.Transactions;

namespace PepsiPSK.Controllers
{
    [Authorize(Roles = "User, Admin")]
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _transactionService.GetTransactions();
            return Ok(transactions);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetTransactionById(Guid guid)
        {
            var transaction = await _transactionService.GetTransactionById(guid);
            return transaction == null ? NotFound() : Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(AddTransactionDto addTransactionDto)
        {
            var adddedTransaction = await _transactionService.AddTransaction(addTransactionDto);
            return Ok(adddedTransaction);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction(UpdateTransactionDto updateTransactionDto)
        {
            var updatedTransaction = await _transactionService.UpdateTransaction(updateTransactionDto);
            return updatedTransaction == null ? NotFound() : Ok(updatedTransaction);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteTransaction(Guid guid)
        {
            var successMessage = await _transactionService.DeleteTransaction(guid);
            return successMessage == null ? NotFound() : Ok(successMessage);
        }
    }
}
