using SimplePSP.Domain.PayableAggregate;

namespace SimplePSP.Domain.Specification
{
    public class DebitCardPayableSpecification : PayableSpecification
    {
        public override PayableStatus CreationStatus => PayableStatus.Paid;
        public override int PaymentDateDaysAfter => 0;
        public override decimal ProcessingFeePercentage => 3;
    }
}