using AutoBogus;
using SimplePSP.API.Models.Request;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Tests.Common.Builders.API
{
    public class CreateTransactionRequestBuilder : AutoFaker<CreateTransactionRequest>
    {
        public CreateTransactionRequestBuilder() : base("pt_BR")
        {
            RuleFor(x => x.Value, f => decimal.Round(f.Random.Decimal(100, 1000), 2))
            .RuleFor(x => x.CardNumber, f => string.Join("", f.Random.Digits(16)))
            .RuleFor(x => x.CardHolderName, f => f.Person.FullName)
            .RuleFor(x => x.PaymentMethod, f => f.PickRandom<PaymentMethod>())
            .RuleFor(x => x.CardSecurityCode, f => string.Join("", f.Random.Digits(3)))
            .RuleFor(x => x.CardValidity, f => f.Date.Future(1));
        }

        public CreateTransactionRequestBuilder WithStoreId(string storeId)
        {
            RuleFor(x => x.StoreId, storeId);
            return this;
        }

        public CreateTransactionRequestBuilder WithValue(decimal value)
        {
            RuleFor(x => x.Value, value);
            return this;
        }

        public CreateTransactionRequestBuilder WithDescription(string description)
        {
            RuleFor(x => x.Description, description);
            return this;
        }

        public CreateTransactionRequestBuilder WithPaymentMethod(PaymentMethod paymentMethod)
        {
            RuleFor(x => x.PaymentMethod, paymentMethod);
            return this;
        }

        public CreateTransactionRequestBuilder WithCardNumber(string cardNumber)
        {
            RuleFor(x => x.CardNumber, cardNumber);
            return this;
        }

        public CreateTransactionRequestBuilder WithCardHolderName(string cardHolderName)
        {
            RuleFor(x => x.CardHolderName, cardHolderName);
            return this;
        }

        public CreateTransactionRequestBuilder WithCardValidity(DateTime cardValidity)
        {
            RuleFor(x => x.CardValidity, cardValidity);
            return this;
        }

        public CreateTransactionRequestBuilder WithCardSecurityCode(string cardSecurityCode)
        {
            RuleFor(x => x.CardSecurityCode, cardSecurityCode);
            return this;
        }
    }
}