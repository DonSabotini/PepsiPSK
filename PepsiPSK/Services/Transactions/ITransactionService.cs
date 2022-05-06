using PepsiPSK.Entities;

namespace PepsiPSK.Services.Transactions
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactions();
        Task<Transaction?> GetTransactionById(Guid guid);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction?> UpdateTransaction(Transaction transaction);
        Task<string?> DeleteTransaction(Guid guid);
    }
}