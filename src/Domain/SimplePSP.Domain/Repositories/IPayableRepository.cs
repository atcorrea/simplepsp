using SimplePSP.Domain.PayableAggregate;

namespace SimplePSP.Domain.Repositories
{
    public interface IPayableRepository
    {
        Task<Payable> Create(Payable payable);

        Task<Payable?> GetById(string id);

        Task<IEnumerable<Payable>> GetAllUnpaidForStore(string storeId);

        Task<IEnumerable<Payable>> GetAllPaidForStore(string storeId);
    }
}