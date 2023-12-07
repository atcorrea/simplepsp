using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SimplePSP.Application.Services;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.Repositories;
using SimplePSP.Tests.Common.Builders.Domain;

namespace SimplePSP.Application.UnitTests.Services
{
    public class StoreBalanceServiceTests
    {
        private AutoMocker _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public async Task GetAvailableBalance_WhenNoErrorOccurs_ShouldReturnSuccessResultWithSumOfPaidPayablesValue()
        {
            const string storeId = "test";
            var paidPayables = new PayableBuilder()
                                .WithTransactionBuilder(new TransactionBuilder()
                                                        .WithStoreId(storeId))
                                .WithValue(10)
                                .WithStatus(PayableStatus.Paid)
                                .Generate(4);

            _mocker.GetMock<IPayableRepository>()
                .Setup(x => x.GetAllPaidForStore(storeId))
                .ReturnsAsync(paidPayables);

            var service = _mocker.CreateInstance<StoreBalanceService>();

            var result = await service.GetAvailableBalance(storeId);

            result.Success.Should().BeTrue();
            result.Value.Should().Be(paidPayables.Sum(x => x.Value));
        }

        [Test]
        public async Task GetAvailableBalance_WhenErrorOccurs_FailedResult()
        {
            const string storeId = "test";

            _mocker.GetMock<IPayableRepository>()
                .Setup(x => x.GetAllPaidForStore(storeId))
                .Throws<Exception>();

            var service = _mocker.CreateInstance<StoreBalanceService>();

            var result = await service.GetAvailableBalance(storeId);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("erro ao obter saldo disponível para loja!");
        }

        [Test]
        public async Task GetIncomingBalance_WhenNoErrorOccurs_ShouldReturnSuccessResultWithSumOfUnpaidPayablesValue()
        {
            const string storeId = "test";
            var unpaidPayables = new PayableBuilder()
                                .WithTransactionBuilder(new TransactionBuilder()
                                                        .WithStoreId(storeId))
                                .WithValue(10)
                                .WithStatus(PayableStatus.WaitingFunds)
                                .Generate(4);

            _mocker.GetMock<IPayableRepository>()
                .Setup(x => x.GetAllUnpaidForStore(storeId))
                .ReturnsAsync(unpaidPayables);

            var service = _mocker.CreateInstance<StoreBalanceService>();

            var result = await service.GetIncomingBalance(storeId);

            result.Success.Should().BeTrue();
            result.Value.Should().Be(unpaidPayables.Sum(x => x.Value));
        }

        [Test]
        public async Task GetIncomingBalance_WhenErrorOccurs_FailedResult()
        {
            const string storeId = "test";

            _mocker.GetMock<IPayableRepository>()
                .Setup(x => x.GetAllUnpaidForStore(storeId))
                .Throws<Exception>();

            var service = _mocker.CreateInstance<StoreBalanceService>();

            var result = await service.GetIncomingBalance(storeId);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("erro ao obter saldo disponível para loja!");
        }
    }
}