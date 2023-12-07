using SimplePSP.Domain.Common;

namespace SimplePSP.Domain.TransactionAggregate
{
    public class Transaction : Entity
    {
        public Transaction(string storeId,
                           decimal value,
                           string description,
                           PaymentMethod paymentMethod,
                           CardNumber cardNumber,
                           string cardHolderName,
                           DateTime cardValidity,
                           string cardSecurityCode,
                           DateTime createDate,
                           string? id = null) : base(id ?? Guid.NewGuid().ToString())
        {
            StoreId = storeId;
            Value = value;
            Description = description;
            PaymentMethod = paymentMethod;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardValidity = cardValidity;
            CardSecurityCode = cardSecurityCode;
            CreateDate = createDate;
        }

        public string StoreId { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public CardNumber CardNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public DateTime CardValidity { get; private set; }
        public string CardSecurityCode { get; private set; }
        public DateTime CreateDate { get; private set; }
    }
}