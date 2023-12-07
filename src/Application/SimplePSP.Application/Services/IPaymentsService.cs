using SimplePSP.Application.Models;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Application.Services
{
    public interface IPaymentsService
    {
        Task<ValueResult<IList<Transaction>>> GetAllTransactionsList();

        Task<Result> StartTransaction(Transaction transaction);
    }
}