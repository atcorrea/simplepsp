using AutoBogus;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Tests.Common.Builders.Domain
{
    public class PayableBuilder : AutoFaker<Payable>
    {
        public PayableBuilder()
        {
            RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => decimal.Round(f.Random.Decimal(100, 1000), 2))
            .RuleFor(x => x.Status, f => f.PickRandom<PayableStatus>())
            .RuleFor(x => x.PaymentDate, f => f.Date.Soon(30))
            .RuleFor(x => x.Transaction, new TransactionBuilder().Generate())
            .RuleFor(x => x.PaymentDate, DateTime.Now);
        }

        public PayableBuilder WithId(string id)
        {
            RuleFor(x => x.Id, id);
            return this;
        }

        public PayableBuilder WithValue(decimal value)
        {
            RuleFor(x => x.Value, value);
            return this;
        }

        public PayableBuilder WithStatus(PayableStatus status)
        {
            RuleFor(x => x.Status, status);
            return this;
        }

        public PayableBuilder WithPaymentDate(DateTime paymentDate)
        {
            RuleFor(x => x.PaymentDate, paymentDate);
            return this;
        }

        public PayableBuilder WithTransaction(Transaction transaction)
        {
            RuleFor(x => x.Transaction, transaction);
            return this;
        }

        public PayableBuilder WithTransactionBuilder(TransactionBuilder transactionBuilder)
        {
            RuleFor(x => x.Transaction, f => transactionBuilder.Generate());
            return this;
        }

        public PayableBuilder WithTransaction(TransactionBuilder transactionBuilder)
        {
            RuleFor(x => x.Transaction, transactionBuilder.Generate());
            return this;
        }

        public PayableBuilder WithCreateDate(DateTime createDate)
        {
            RuleFor(x => x.CreateDate, createDate);
            return this;
        }
    }
}