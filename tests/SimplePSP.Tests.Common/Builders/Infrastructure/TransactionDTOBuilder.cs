using AutoBogus;
using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Infrastructure.Persistence.DTOs;

namespace SimplePSP.Tests.Common.Builders.Infrastructure
{
    public class TransactionDTOBuilder : AutoFaker<TransactionDTO>
    {
        public TransactionDTOBuilder()
        {
            RuleFor(x => x.Value, f => decimal.Round(f.Random.Decimal(100, 1000), 2))
            .RuleFor(x => x.CardNumber, f => string.Join("", f.Random.Digits(16)))
            .RuleFor(x => x.CardSecurityCode, f => string.Join("", f.Random.Digits(3)))
            .RuleFor(x => x.CardValidity, f => f.Date.Future(1))
            .RuleFor(x => x.PaymentMethod, f => f.PickRandom<PaymentMethod>().ToString())
            .RuleFor(x => x.CreateDate, DateTime.Now);
        }

        public TransactionDTOBuilder WithId(string id)
        {
            RuleFor(x => x.Id, id);
            return this;
        }

        public TransactionDTOBuilder WithStoreId(string storeId)
        {
            RuleFor(x => x.StoreId, storeId);
            return this;
        }

        public TransactionDTOBuilder WithValue(decimal value)
        {
            RuleFor(x => x.Value, value);
            return this;
        }

        public TransactionDTOBuilder WithDescription(string description)
        {
            RuleFor(x => x.Description, description);
            return this;
        }

        public TransactionDTOBuilder WithPaymentMethod(PaymentMethod paymentMethod)
        {
            RuleFor(x => x.PaymentMethod, paymentMethod.ToString());
            return this;
        }

        public TransactionDTOBuilder WithCardNumber(string cardNumber)
        {
            RuleFor(x => x.CardNumber, cardNumber);
            return this;
        }

        public TransactionDTOBuilder WithCardHolderName(string cardHolderName)
        {
            RuleFor(x => x.CardHolderName, cardHolderName);
            return this;
        }

        public TransactionDTOBuilder WithCardValidity(DateTime cardValidity)
        {
            RuleFor(x => x.CardValidity, cardValidity);
            return this;
        }

        public TransactionDTOBuilder WithCardSecurityCode(string cardSecurityCode)
        {
            RuleFor(x => x.CardSecurityCode, cardSecurityCode);
            return this;
        }

        public TransactionDTOBuilder WithCreateDate(DateTime createDate)
        {
            RuleFor(x => x.CreateDate, createDate);
            return this;
        }
    }
}