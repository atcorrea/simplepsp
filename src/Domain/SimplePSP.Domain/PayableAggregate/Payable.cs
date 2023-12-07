using SimplePSP.Domain.Common;
using SimplePSP.Domain.Specification;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Domain.PayableAggregate
{
    public class Payable : Entity
    {
        public Payable(string id, PayableStatus status, DateTime paymentDate, decimal value, Transaction transaction, DateTime createDate) : base(id)
        {
            Status = status;
            PaymentDate = paymentDate;
            Value = value;
            Transaction = transaction;
            CreateDate = createDate;
        }

        public PayableStatus Status { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public decimal Value { get; private set; }
        public Transaction Transaction { get; private set; }
        public DateTime CreateDate { get; set; }

        public static Payable CreateFromTransaction(Transaction transaction)
        {
            var specification = PayableSpecificationFactory.GetSpecification(transaction.PaymentMethod);
            return specification.GetPayableForTransaction(transaction);
        }
    }
}