using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;

        public TransactionService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            return await _context.Transactions.Select(transaction => transaction).ToListAsync();
        }

        public async Task<Transaction?> GetTransactionById(Guid guid)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == guid);
            return transaction ?? null;
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            var addedTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);
            return addedTransaction;
        }

        public async Task<Transaction?> UpdateTransaction(Transaction transaction)
        {
            var transactionToUpdate = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);

            if (transactionToUpdate == null)
            {
                return null;
            }

            transactionToUpdate.FlowerId = transaction.FlowerId;
            transactionToUpdate.Diference = transaction.Diference;
            await _context.SaveChangesAsync();
            return transactionToUpdate;
        }

        public async Task<string?> DeleteTransaction(Guid guid)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == guid);

            if (transaction == null)
            {
                return null;
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return "Successfully deleted!";
        }
    }
}
