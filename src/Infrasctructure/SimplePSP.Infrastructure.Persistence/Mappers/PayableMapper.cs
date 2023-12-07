using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Infrastructure.Persistence.DTOs;

namespace SimplePSP.Infrastructure.Persistence.Mappers
{
    public class PayableMapper
    {
        public static PayableDTO ToDTO(Payable payable)
        {
            return new()
            {
                Id = payable.Id,
                PaymentDate = payable.PaymentDate,
                Status = payable.Status.ToString(),
                Transaction = TransactionMapper.ToDTO(payable.Transaction),
                Value = payable.Value,
                CreateDate = payable.CreateDate,
            };
        }

        public static Payable ToDomain(PayableDTO dto)
        {
            return new(id: dto.Id,
                       status: Enum.Parse<PayableStatus>(dto.Status),
                       paymentDate: dto.PaymentDate,
                       value: dto.Value,
                       transaction: TransactionMapper.ToDomain(dto.Transaction),
                       createDate: dto.CreateDate);
        }
    }
}