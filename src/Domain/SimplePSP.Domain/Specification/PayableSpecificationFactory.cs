using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.Domain.Specification
{
    public static class PayableSpecificationFactory
    {
        public static PayableSpecification GetSpecification(PaymentMethod paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethod.DebitCard => new DebitCardPayableSpecification(),
                PaymentMethod.CreditCard => new CreditCardPayableSpecification(),
                _ => throw new NotImplementedException("tipo de pagamento inválido!"),
            };
        }
    }
}