#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;

namespace SimplePSP.Infrastructure.Persistence.DTOs
{
    [Table("Payables")]
    public class PayableDTO
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Value { get; set; }
        public TransactionDTO Transaction { get; set; }
        public DateTime CreateDate { get; set; }
    }
}