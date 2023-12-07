using Microsoft.Extensions.Logging;
using SimplePSP.Application.Exceptions;
using SimplePSP.Application.Models;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.Repositories;
using SimplePSP.Domain.TransactionAggregate;
using System.Text.Json;

namespace SimplePSP.Application.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPayableRepository _payableRepository;
        private readonly ILogger<PaymentsService> _logger;

        public PaymentsService(ITransactionRepository transactionRepository,
                               IPayableRepository payableRepository,
                               ILogger<PaymentsService> logger)
        {
            _transactionRepository = transactionRepository;
            _payableRepository = payableRepository;
            _logger = logger;
        }

        public async Task<Result> StartTransaction(Transaction transaction)
        {
            try
            {
                _logger.LogInformation("Salvando transação");
                await _transactionRepository.Create(transaction);

                var payableForTransaction = Payable.CreateFromTransaction(transaction);

                _logger.LogInformation("Criando recebível para transação");
                await _payableRepository.Create(payableForTransaction);
            }
            catch (DataBaseException ex)
            {
                _logger.LogDebug("{entity}", JsonSerializer.Serialize(ex.Entity));
                return Fail(ex, "ocorreu um erro ao iniciar a transação!");
            }
            catch (Exception ex)
            {
                return Fail(ex, "erro crítico ao iniciar a transação!");
            }

            return Result.CreateSuccessful();
        }

        public async Task<ValueResult<IList<Transaction>>> GetAllTransactionsList()
        {
            _logger.LogInformation("Obtendo lista de transações");
            try
            {
                var transactions = await _transactionRepository.GetAll();
                return ValueResult<IList<Transaction>>.CreateSuccessful(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);
                return ValueResult<IList<Transaction>>.CreateFailed("Erro ao retornar lista de transações!");
            }
        }

        private Result Fail(Exception ex, string message)
        {
            _logger.LogError(ex, "{message}", message);
            return Result.CreateFailed(message);
        }
    }
}