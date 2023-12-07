using Microsoft.Extensions.Logging;
using SimplePSP.Application.Models;
using SimplePSP.Domain.Repositories;

namespace SimplePSP.Application.Services
{
    public class StoreBalanceService : IStoreBalanceService
    {
        private readonly IPayableRepository _payableRepository;
        private readonly ILogger<StoreBalanceService> _logger;

        public StoreBalanceService(IPayableRepository payableRepository,
                                   ILogger<StoreBalanceService> logger)
        {
            _payableRepository = payableRepository;
            _logger = logger;
        }

        public async Task<ValueResult<decimal>> GetAvailableBalance(string storeId)
        {
            _logger.LogInformation("Obtendo saldo disponível da loja");

            try
            {
                var payables = await _payableRepository.GetAllPaidForStore(storeId);
                var sum = payables.Sum(x => x.Value);

                return ValueResult<decimal>.CreateSuccessful(sum);
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
                return ValueResult<decimal>.CreateFailed("erro ao obter saldo disponível para loja!");
            }
        }

        public async Task<ValueResult<decimal>> GetIncomingBalance(string storeId)
        {
            _logger.LogInformation("Obtendo saldo a receber do loja");

            try
            {
                var payables = await _payableRepository.GetAllUnpaidForStore(storeId);
                var sum = payables.Sum(x => x.Value);

                return ValueResult<decimal>.CreateSuccessful(sum);
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
                return ValueResult<decimal>.CreateFailed("erro ao obter saldo disponível para loja!");
            }
        }
    }
}