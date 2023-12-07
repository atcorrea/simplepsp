using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SimplePSP.Application.Exceptions;
using SimplePSP.Application.Services;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.Repositories;
using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Tests.Common.Builders.Domain;

namespace SimplePSP.Application.UnitTests.Services
{
    public class PaymentsServiceTests
    {
        private AutoMocker _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public async Task StartTransaction_WhenReceivingTransaction_ShouldCreateNewTransaction()
        {
            var transaction = new TransactionBuilder().Generate();
            var service = _mocker.CreateInstance<PaymentsService>();

            await service.StartTransaction(transaction);

            _mocker.GetMock<ITransactionRepository>()
                .Verify(x => x.Create(It.IsAny<Transaction>()), Times.Once());
        }

        [Test]
        public async Task StartTransaction_WhenNoExceptionOccurs_ShouldReturnSuccessResult()
        {
            var transaction = new TransactionBuilder().Generate();
            var service = _mocker.CreateInstance<PaymentsService>();

            var result = await service.StartTransaction(transaction);

            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task StartTransaction_WhenTransactionIsCreated_ShouldCreateNewPayable()
        {
            var transaction = new TransactionBuilder().Generate();
            var service = _mocker.CreateInstance<PaymentsService>();

            await service.StartTransaction(transaction);

            _mocker.GetMock<IPayableRepository>()
                .Verify(x => x.Create(It.IsAny<Payable>()), Times.Once());
        }

        [Test]
        public async Task StartTransaction_WhenCreateTransactionFails_ShouldReturnFailedResult()
        {
            var transaction = new TransactionBuilder().Generate();
            var service = _mocker.CreateInstance<PaymentsService>();

            _mocker.GetMock<ITransactionRepository>()
                .Setup(x => x.Create(transaction)).Throws(new DataBaseException("erro!", null));

            var result = await service.StartTransaction(transaction);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("ocorreu um erro ao iniciar a transação!");
        }

        [Test]
        public async Task StartTransaction_WhenCreatePayableFails_ShouldReturnFailedResult()
        {
            var transaction = new TransactionBuilder().Generate();
            var service = _mocker.CreateInstance<PaymentsService>();

            _mocker.GetMock<IPayableRepository>()
                .Setup(x => x.Create(It.Is<Payable>(p => p.Transaction == transaction))).Throws(new DataBaseException("erro!", null));

            var result = await service.StartTransaction(transaction);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("ocorreu um erro ao iniciar a transação!");
        }

        [Test]
        public async Task GetAllTransactionsList_WhenNoErrorOccurs_ShouldReturnSuccessResult()
        {
            var service = _mocker.CreateInstance<PaymentsService>();
            var transactions = new TransactionBuilder().Generate(3);

            _mocker.GetMock<ITransactionRepository>()
                .Setup(x => x.GetAll())
                .ReturnsAsync(transactions);

            var result = await service.GetAllTransactionsList();

            result.Success.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(transactions);
        }

        [Test]
        public async Task GetAllTransactionsList_WhenErrorOccurs_ShouldReturnFailedResult()
        {
            var service = _mocker.CreateInstance<PaymentsService>();

            _mocker.GetMock<ITransactionRepository>()
                .Setup(x => x.GetAll())
                .Throws<Exception>();

            var result = await service.GetAllTransactionsList();

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("Erro ao retornar lista de transações!");
        }
    }
}