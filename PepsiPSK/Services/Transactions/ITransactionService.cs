using PepsiPSK.Entities;
using PepsiPSK.Models.Transaction;

namespace PepsiPSK.Services.Transactions
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactions();
        Task<Transaction?> GetTransactionById(Guid guid);
        Task<Transaction> AddTransaction(AddTransactionDto addTransactionDto);
        Task<Transaction?> UpdateTransaction(UpdateTransactionDto updateTransactionDto);
        Task<string?> DeleteTransaction(Guid guid);
    }
}