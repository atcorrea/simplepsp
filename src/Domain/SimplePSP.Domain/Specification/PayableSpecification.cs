using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Domain.Specification
{
    public abstract class PayableSpecification
    {
        public abstract PayableStatus CreationStatus { get; }
        public abstract int PaymentDateDaysAfter { get; }
        public abstract decimal ProcessingFeePercentage { get; }

        public Payable GetPayableForTransaction(Transaction transaction)
        {
            var calculatedPaymentDate = transaction.CreateDate.AddDays(PaymentDateDaysAfter);
            decimal calculatedPayableValue = decimal.Round(transaction.Value * ((100m - ProcessingFeePercentage) / 100m), 2);

            return new(Guid.NewGuid().ToString(),
                       CreationStatus,
                       calculatedPaymentDate,
                       calculatedPayableValue,
                       transaction,
                       DateTime.Now);
        }
    }
}