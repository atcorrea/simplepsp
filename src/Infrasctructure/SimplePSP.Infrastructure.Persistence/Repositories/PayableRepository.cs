using Microsoft.EntityFrameworkCore;
using SimplePSP.Application.Exceptions;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.Repositories;
using SimplePSP.Infrastructure.Persistence.Mappers;

namespace SimplePSP.Infrastructure.Persistence.Repositories
{
    public class PayableRepository : IPayableRepository
    {
        private readonly SimplePSPContext _dbContext;

        public PayableRepository(SimplePSPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payable> Create(Payable payable)
        {
            try
            {
                var dto = PayableMapper.ToDTO(payable);
                _dbContext.Entry(dto.Transaction).State = EntityState.Unchanged;

                _dbContext.Payables.Add(dto);

                await CommitAsync();

                return payable;
            }
            catch (Exception ex)
            {
                throw new DataBaseException(payable, "ocorreu um erro ao criar o payable!", ex);
            }
        }

        public async Task<IEnumerable<Payable>> GetAllPaidForStore(string storeId)
        {
            var paidPayables = new List<Payable>();
            var paidPayablesDtos = await _dbContext.Payables
                                            .AsNoTracking()
                                            .Include(x => x.Transaction)
                                            .Where(x => x.Transaction.StoreId == storeId
                                                        && x.Status == PayableStatus.Paid.ToString())
                                            .ToListAsync();

            foreach (var dto in paidPayablesDtos)
                paidPayables.Add(PayableMapper.ToDomain(dto));

            return paidPayables;
        }

        public async Task<IEnumerable<Payable>> GetAllUnpaidForStore(string storeId)
        {
            var unpaidPayables = new List<Payable>();
            var unpaidPayablesDtos = await _dbContext.Payables
                                            .AsNoTracking()
                                            .Include(x => x.Transaction)
                                            .Where(x => x.Transaction.StoreId == storeId
                                                        && x.Status == PayableStatus.WaitingFunds.ToString())
                                            .ToListAsync();

            foreach (var dto in unpaidPayablesDtos)
                unpaidPayables.Add(PayableMapper.ToDomain(dto));

            return unpaidPayables;
        }

        public async Task<Payable?> GetById(string id)
        {
            var dto = await _dbContext.Payables.FirstOrDefaultAsync(x => x.Id == id);
            if (dto == null)
                return null;

            return PayableMapper.ToDomain(dto);
        }

        private async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();
        }
    }
}