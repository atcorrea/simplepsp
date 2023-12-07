using AutoBogus;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Tests.Common.Builders.Domain
{
    public class TransactionBuilder : AutoFaker<Transaction>
    {
        public TransactionBuilder()
        {
            RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => decimal.Round(f.Random.Decimal(100, 1000), 2))
            .RuleFor(x => x.CardNumber, f => new CardNumber(string.Join("", f.Random.Digits(16))))
            .RuleFor(x => x.PaymentMethod, f => f.PickRandom<PaymentMethod>())
            .RuleFor(x => x.CardSecurityCode, f => string.Join("", f.Random.Digits(3)))
            .RuleFor(x => x.CardValidity, f => f.Date.Future(1))
            .RuleFor(x => x.CreateDate, DateTime.Now);
        }

        public TransactionBuilder WithId(string id)
        {
            RuleFor(x => x.Id, id);
            return this;
        }

        public TransactionBuilder WithStoreId(string storeId)
        {
            RuleFor(x => x.StoreId, storeId);
            return this;
        }

        public TransactionBuilder WithValue(decimal value)
        {
            RuleFor(x => x.Value, value);
            return this;
        }

        public TransactionBuilder WithDescription(string description)
        {
            RuleFor(x => x.Description, description);
            return this;
        }

        public TransactionBuilder WithPaymentMethod(PaymentMethod paymentMethod)
        {
            RuleFor(x => x.PaymentMethod, paymentMethod);
            return this;
        }

        public TransactionBuilder WithCardNumber(string cardNumber)
        {
            RuleFor(x => x.CardNumber, new CardNumber(cardNumber));
            return this;
        }

        public TransactionBuilder WithCardHolderName(string cardHolderName)
        {
            RuleFor(x => x.CardHolderName, cardHolderName);
            return this;
        }

        public TransactionBuilder WithCardValidity(DateTime cardValidity)
        {
            RuleFor(x => x.CardValidity, cardValidity);
            return this;
        }

        public TransactionBuilder WithCardSecurityCode(string cardSecurityCode)
        {
            RuleFor(x => x.CardSecurityCode, cardSecurityCode);
            return this;
        }

        public TransactionBuilder WithCreateDate(DateTime createDate)
        {
            RuleFor(x => x.CreateDate, createDate);
            return this;
        }
    }
}