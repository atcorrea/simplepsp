#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;

namespace SimplePSP.Infrastructure.Persistence.DTOs
{
    [Table("Transactions")]
    public class TransactionDTO
    {
        public string Id { get; set; }
        public string StoreId { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardValidity { get; set; }
        public string CardSecurityCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}