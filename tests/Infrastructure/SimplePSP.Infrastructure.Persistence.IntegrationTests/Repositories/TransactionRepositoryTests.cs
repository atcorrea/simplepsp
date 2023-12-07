using FluentAssertions;
using SimplePSP.Infrastructure.Persistence.Mappers;
using SimplePSP.Infrastructure.Persistence.Repositories;
using SimplePSP.Tests.Common.Builders.Domain;

namespace SimplePSP.Infrastructure.Persistence.IntegrationTests.Repositories
{
    public class TransactionRepositoryTests : TestBase
    {
        [Test]
        public async Task Create_WhenReceivingTransaction_ShouldAddItToDbContext()
        {
            var repo = new TransactionRepository(Context);
            var transaction = new TransactionBuilder().Generate();

            await repo.Create(transaction);

            var savedTransactionDTO = Context.Transactions.First();
            var savedTransaction = TransactionMapper.ToDomain(savedTransactionDTO);

            savedTransaction.Should().BeEquivalentTo(transaction, opt => opt.Excluding(x => x.CardNumber));
        }

        [Test]
        public async Task Create_WhenReceivingTransaction_ShouldSaveOnlyLast4DigitsOfCreditCardNumber()
        {
            var repo = new TransactionRepository(Context);
            var transaction = new TransactionBuilder().Generate();

            await repo.Create(transaction);

            var savedTransactionDTO = Context.Transactions.First();
            var savedTransaction = TransactionMapper.ToDomain(savedTransactionDTO);

            savedTransaction.CardNumber.Number.Should().Be(transaction.CardNumber.Last4Digits.PadLeft(16, '*'));
            savedTransaction.CardNumber.Last4Digits.Should().Be(transaction.CardNumber.Last4Digits);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllCreatedTransactions()
        {
            const int count = 4;
            var repo = new TransactionRepository(Context);
            var transactions = new TransactionBuilder().Generate(count);

            foreach (var transaction in transactions)
                await repo.Create(transaction);

            var savedTransactions = await repo.GetAll();

            savedTransactions.Count.Should().Be(count);
            savedTransactions.Should().ContainEquivalentOf(transactions[0], opt => opt.Excluding(x => x.CardNumber));
        }
    }
}