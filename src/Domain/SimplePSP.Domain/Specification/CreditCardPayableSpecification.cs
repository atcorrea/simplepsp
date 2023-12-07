using SimplePSP.Domain.PayableAggregate;

namespace SimplePSP.Domain.Specification
{
    public class CreditCardPayableSpecification : PayableSpecification
    {
        public override PayableStatus CreationStatus => PayableStatus.WaitingFunds;
        public override int PaymentDateDaysAfter => 30;
        public override decimal ProcessingFeePercentage => 5;
    }
}