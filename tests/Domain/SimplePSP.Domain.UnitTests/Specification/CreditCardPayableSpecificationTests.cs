using FluentAssertions;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Tests.Common.Builders.Domain;

namespace SimplePSP.Domain.UnitTests.Specification
{
    public class CreditCardPayableSpecificationTests
    {
        [Test]
        public void GetPayableForTransaction_WhenReceivingTransaction_ShouldSetStatusToWaitingFunds()
        {
            var transaction = new TransactionBuilder()
                .WithPaymentMethod(PaymentMethod.CreditCard)
                .Generate();

            var payable = Payable.CreateFromTransaction(transaction);

            payable.Status.Should().Be(PayableStatus.WaitingFunds);
        }

        [Test]
        public void GetPayableForTransaction_WhenReceivingTransaction_ShouldSetPaymentDateToTransactionDatePlus30Days()
        {
            var transaction = new TransactionBuilder()
                .WithPaymentMethod(PaymentMethod.CreditCard)
                .WithCreateDate(DateTime.Now)
                .Generate();

            var payable = Payable.CreateFromTransaction(transaction);

            payable.PaymentDate.Should().BeCloseTo(transaction.CreateDate.AddDays(30), TimeSpan.FromSeconds(1));
        }

        [TestCase(100, 95)]
        [TestCase(50, 47.5)]
        [TestCase(29.99, 28.49)]
        public void GetPayableForTransaction_WhenReceivingTransaction_ShouldSetValueToTransactionValueMinus5Percent(decimal transactionValue, decimal expectedPayableValue)
        {
            var transaction = new TransactionBuilder()
                .WithPaymentMethod(PaymentMethod.CreditCard)
                .WithValue(transactionValue)
                .Generate();

            var payable = Payable.CreateFromTransaction(transaction);

            payable.Value.Should().Be(expectedPayableValue);
        }
    }
}