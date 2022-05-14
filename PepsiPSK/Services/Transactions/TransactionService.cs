using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Transaction;
using PepsiPSK.Services.Users;
using PepsiPSK.Utils.Authentication;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfoRetriever _currentUserInfoRetriever;

        public TransactionService(DataContext context, IMapper mapper, IUserService userService, ICurrentUserInfoRetriever currentUserInfoRetriever)
        {
            _context = context;
            _mapper = mapper;
            _currentUserInfoRetriever = currentUserInfoRetriever;
        }

        private string GetCurrentUserId()
        {
            return _currentUserInfoRetriever.RetrieveCurrentUserId();
        }

        private bool AdminCheck()
        {
            return _currentUserInfoRetriever.CheckIfCurrentUserIsAdmin();
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            return AdminCheck() ? await _context.Transactions.Select(transaction => transaction).ToListAsync() : await _context.Transactions.Where(t => t.UserId == GetCurrentUserId()).ToListAsync(); ;
        }

        public async Task<Transaction?> GetTransactionById(Guid guid)
        {
            var transaction = AdminCheck() ? await _context.Transactions.FirstOrDefaultAsync(t => t.Id == guid) : await _context.Transactions.FirstOrDefaultAsync(t => t.Id == guid && t.UserId == GetCurrentUserId());
            return transaction ?? null;
        }

        public async Task<Transaction> AddTransaction(AddTransactionDto addTransactionDto)
        {
            Transaction newTransaction = _mapper.Map<Transaction>(addTransactionDto);
            newTransaction.UserId = GetCurrentUserId();
            await _context.Transactions.AddAsync(newTransaction);
            await _context.SaveChangesAsync();
            var addedTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == newTransaction.Id);
            return addedTransaction;
        }

        public async Task<Transaction?> UpdateTransaction(UpdateTransactionDto updateTransactionDto)
        {
            var transactionToUpdate = AdminCheck() ? await _context.Transactions.FirstOrDefaultAsync(t => t.Id == updateTransactionDto.Id) : await _context.Transactions.FirstOrDefaultAsync(t => t.Id == updateTransactionDto.Id && t.UserId == GetCurrentUserId());

            if (transactionToUpdate == null)
            {
                return null;
            }

            transactionToUpdate.FlowerId = updateTransactionDto.FlowerId;
            transactionToUpdate.Diference = updateTransactionDto.Diference;
            await _context.SaveChangesAsync();
            return transactionToUpdate;
        }

        public async Task<string?> DeleteTransaction(Guid guid)
        {
            var transaction = AdminCheck() ? await _context.Transactions.FirstOrDefaultAsync(t => t.Id == guid) : await _context.Transactions.FirstOrDefaultAsync(t => t.Id == guid && t.UserId == GetCurrentUserId());

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
