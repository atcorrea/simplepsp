using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Infrastructure.Persistence.Mappers;
using SimplePSP.Infrastructure.Persistence.Repositories;
using SimplePSP.Tests.Common.Builders.Domain;

namespace SimplePSP.Infrastructure.Persistence.IntegrationTests.Repositories
{
    public class PayableRepositoryTests : TestBase
    {
        [Test]
        public async Task Create_WhenReceivingPayableOfExistingTransaction_ShouldAddItToDbContext()
        {
            var transaction = new TransactionBuilder().Generate();
            var transactionRepo = new TransactionRepository(Context);
            await transactionRepo.Create(transaction);

            var payableRepo = new PayableRepository(Context);
            var payable = new PayableBuilder().WithTransaction(transaction).Generate();

            await payableRepo.Create(payable);

            var savedPayableDTO = Context.Payables.AsNoTracking().Include(x => x.Transaction).First();
            var savedPayable = PayableMapper.ToDomain(savedPayableDTO);

            savedPayable.Should().BeEquivalentTo(payable, opt => opt.Excluding(x => x.Transaction.CardNumber));
        }

        [Test]
        public async Task GetAllPaidForStore_WhenThereArePayedPayablesInDB_ShouldReturnListWithThem()
        {
            const string storeId = "111111111111111";

            var payableRepo = new PayableRepository(Context);
            var paidPayables = new PayableBuilder()
                .WithStatus(PayableStatus.Paid)
                .WithTransactionBuilder(new TransactionBuilder()
                                        .WithStoreId(storeId))
                .Generate(4);
            var unpaidPayables = new PayableBuilder()
                .WithStatus(PayableStatus.WaitingFunds)
                .WithTransactionBuilder(new TransactionBuilder()
                                .WithStoreId(storeId))
                .Generate(2);

            foreach (var payable in paidPayables.Union(unpaidPayables))
                Context.Add(PayableMapper.ToDTO(payable));

            await Context.SaveChangesAsync();

            var dbPaid = await payableRepo.GetAllPaidForStore(storeId);

            dbPaid.Count().Should().Be(paidPayables.Count);
        }

        [Test]
        public async Task GetAllUnpaidForStore_WhenThereAreUnpaidPayablesInDB_ShouldReturnListWithThem()
        {
            const string storeId = "111111111111111";

            var repo = new PayableRepository(Context);
            var paidPayables = new PayableBuilder()
                .WithStatus(PayableStatus.Paid)
                .WithTransactionBuilder(new TransactionBuilder()
                                        .WithStoreId(storeId))
                .Generate(4);
            var unpaidPayables = new PayableBuilder()
                .WithStatus(PayableStatus.WaitingFunds)
                .WithTransactionBuilder(new TransactionBuilder()
                                .WithStoreId(storeId))
                .Generate(2);

            foreach (var payable in paidPayables.Union(unpaidPayables))
                Context.Add(PayableMapper.ToDTO(payable));

            await Context.SaveChangesAsync();

            var dbPaid = await repo.GetAllUnpaidForStore(storeId);

            dbPaid.Count().Should().Be(unpaidPayables.Count);
        }
    }
}