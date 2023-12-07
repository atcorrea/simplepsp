using AutoBogus;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Infrastructure.Persistence.DTOs;

namespace SimplePSP.Tests.Common.Builders.Infrastructure
{
    public class PayableDTOBuilder : AutoFaker<PayableDTO>
    {
        public PayableDTOBuilder()
        {
            RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => decimal.Round(f.Random.Decimal(100, 1000), 2))
            .RuleFor(x => x.Status, f => f.PickRandom<PayableStatus>().ToString())
            .RuleFor(x => x.PaymentDate, f => f.Date.Soon(30))
            .RuleFor(x => x.Transaction, new TransactionDTOBuilder().Generate())
            .RuleFor(x => x.CreateDate, DateTime.Now);
        }

        public PayableDTOBuilder WithId(string id)
        {
            RuleFor(x => x.Id, id);
            return this;
        }

        public PayableDTOBuilder WithValue(decimal value)
        {
            RuleFor(x => x.Value, value);
            return this;
        }

        public PayableDTOBuilder WithStatus(PayableStatus status)
        {
            RuleFor(x => x.Status, status.ToString());
            return this;
        }

        public PayableDTOBuilder WithPaymentDate(DateTime paymentDate)
        {
            RuleFor(x => x.PaymentDate, paymentDate);
            return this;
        }

        public PayableDTOBuilder WithTransaction(TransactionDTO transaction)
        {
            RuleFor(x => x.Transaction, transaction);
            return this;
        }

        public PayableDTOBuilder WithCreateDate(DateTime createDate)
        {
            RuleFor(x => x.CreateDate, createDate);
            return this;
        }
    }
}