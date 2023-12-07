#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.API.Models.Response
{
    public class GetTransactionResponse
    {
        public string Id { get; private set; }
        public string StoreId { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }
        public string PaymentMethod { get; private set; }
        public string CardNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public string CardValidity { get; private set; }
        public string CardSecurityCode { get; private set; }

        public static GetTransactionResponse CreateFromTransaction(Transaction transaction)
        {
            return new()
            {
                Id = transaction.Id,
                StoreId = transaction.StoreId,
                Value = transaction.Value,
                Description = transaction.Description,
                PaymentMethod = transaction.PaymentMethod.ToString(),
                CardNumber = transaction.CardNumber.Number,
                CardHolderName = transaction.CardHolderName,
                CardValidity = transaction.CardValidity.ToString("MM/yy"),
                CardSecurityCode = transaction.CardSecurityCode,
            };
        }
    }
}