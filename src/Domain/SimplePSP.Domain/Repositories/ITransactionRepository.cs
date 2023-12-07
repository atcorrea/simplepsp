using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> Create(Transaction transaction);

        Task<Transaction?> GetById(string transactionId);

        Task<IList<Transaction>> GetAll();
    }
}