using Microsoft.EntityFrameworkCore;
using SimplePSP.Application.Exceptions;
using SimplePSP.Domain.Repositories;
using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Infrastructure.Persistence.Mappers;

namespace SimplePSP.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SimplePSPContext _dbContext;

        public TransactionRepository(SimplePSPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            try
            {
                var dto = TransactionMapper.ToDTO(transaction);
                _dbContext.Add(dto);
                await CommitAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                throw new DataBaseException(transaction, "erro ao criar transação!", ex);
            }
        }

        public async Task<IList<Transaction>> GetAll()
        {
            try
            {
                return await _dbContext.Transactions
                    .Select(x => TransactionMapper.ToDomain(x))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("erro ao buscar lista de transações!", ex);
            }
        }

        public async Task<Transaction?> GetById(string transactionId)
        {
            try
            {
                var dto = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);

                if (dto == null)
                    return null;

                return TransactionMapper.ToDomain(dto);
            }
            catch (Exception ex)
            {
                throw new DataBaseException($"erro ao buscar transação {transactionId}", ex);
            }
        }

        private async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();
        }
    }
}