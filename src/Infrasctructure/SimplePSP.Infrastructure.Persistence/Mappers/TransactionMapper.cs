using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Infrastructure.Persistence.DTOs;

namespace SimplePSP.Infrastructure.Persistence.Mappers
{
    public class TransactionMapper
    {
        public static TransactionDTO ToDTO(Transaction domain)
        {
            return new TransactionDTO()
            {
                Id = domain.Id,
                Value = domain.Value,
                StoreId = domain.StoreId,
                Description = domain.Description,
                CardHolderName = domain.CardHolderName,
                CardNumber = domain.CardNumber.Last4Digits,
                CardSecurityCode = domain.CardSecurityCode,
                CardValidity = domain.CardValidity,
                PaymentMethod = domain.PaymentMethod.ToString(),
                CreateDate = domain.CreateDate,
            };
        }

        public static Transaction ToDomain(TransactionDTO dto)
        {
            return new Transaction(storeId: dto.StoreId,
                                          value: dto.Value,
                                          description: dto.Description,
                                          paymentMethod: Enum.Parse<PaymentMethod>(dto.PaymentMethod),
                                          cardNumber: new CardNumber(dto.CardNumber.PadLeft(16, '*')),
                                          cardHolderName: dto.CardHolderName,
                                          cardValidity: dto.CardValidity,
                                          cardSecurityCode: dto.CardSecurityCode,
                                          id: dto.Id,
                                          createDate: dto.CreateDate);
        }
    }
}