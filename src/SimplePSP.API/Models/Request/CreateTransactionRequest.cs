#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.API.Models.Request
{
    public class CreateTransactionRequest
    {
        /// <summary>
        /// Identificador do cliente
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Valor da transação
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Descrição da transação
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Método de pagamento
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Número do cartão
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Nome do portador do cartão
        /// </summary>
        public string CardHolderName { get; set; }

        /// <summary>
        /// Data de validade do cartão
        /// </summary>
        public DateTime CardValidity { get; set; }

        /// <summary>
        /// Código de verificação do cartão (CVV)
        /// </summary>
        public string CardSecurityCode { get; set; }
    }
}