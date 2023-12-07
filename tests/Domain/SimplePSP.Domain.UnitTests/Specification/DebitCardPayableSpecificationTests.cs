using FluentAssertions;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Tests.Common.Builders.Domain;

namespace SimplePSP.Domain.UnitTests.Specification
{
    public class DebitCardPayableSpecificationTests
    {
        [Test]
        public void GetPayableForTransaction_WhenReceivingTransaction_ShouldSetStatusToPaid()
        {
            var transaction = new TransactionBuilder()
                .WithPaymentMethod(PaymentMethod.DebitCard)
                .Generate();

            var payable = Payable.CreateFromTransaction(transaction);

            payable.Status.Should().Be(PayableStatus.Paid);
        }

        [Test]
        public void GetPayableForTransaction_WhenReceivingTransaction_ShouldSetPaymentDateToTransactionDate()
        {
            var transaction = new TransactionBuilder()
                .WithPaymentMethod(PaymentMethod.DebitCard)
                .WithCreateDate(DateTime.Now)
                .Generate();

            var payable = Payable.CreateFromTransaction(transaction);

            payable.PaymentDate.Should().BeCloseTo(transaction.CreateDate, TimeSpan.FromSeconds(1));
        }

        [TestCase(100, 97)]
        [TestCase(50, 48.5)]
        [TestCase(29.99, 29.09)]
        public void GetPayableForTransaction_WhenReceivingTransaction_ShouldSetValueToTransactionValueMinus3Percent(decimal transactionValue, decimal expectedPayableValue)
        {
            var transaction = new TransactionBuilder()
                .WithPaymentMethod(PaymentMethod.DebitCard)
                .WithValue(transactionValue)
                .Generate();

            var payable = Payable.CreateFromTransaction(transaction);

            payable.Value.Should().Be(expectedPayableValue);
        }
    }
}