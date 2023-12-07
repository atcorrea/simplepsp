using SimplePSP.Application.Models;

namespace SimplePSP.Application.Services
{
    public interface IStoreBalanceService
    {
        Task<ValueResult<decimal>> GetAvailableBalance(string storeId);

        Task<ValueResult<decimal>> GetIncomingBalance(string storeId);
    }
}