using FluentValidation;
using SimplePSP.API.Models.Request;

namespace SimplePSP.API.Validators
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.StoreId)
                .NotEmpty()
                .WithMessage("O campo StoreId deve ser informado");

            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Valor da transação deve maior que zero");

            RuleFor(x => x.CardNumber)
                .NotNull().WithMessage("Número do cartão deve ser informado")
                .Matches(@"^\d{4}[\s\-]?\d{4}[\s\-]?\d{4}[\s\-]?\d{4}$").WithMessage("Número do cartão deve possuir 16 números");

            RuleFor(x => x.CardHolderName)
                .Length(10, 40)
                .WithMessage("Nome do portador deve possuir entre 10 e 40 caracteres");

            var currentDate = DateTime.Now;
            var begginingOfCurrentMonth = new DateTime(year: currentDate.Year,
                                                       month: currentDate.Month,
                                                       day: 1);
            RuleFor(x => x.CardValidity)
                .GreaterThanOrEqualTo(begginingOfCurrentMonth)
                .WithMessage("A data de validade deve ser no mínimo o final do mês corrente");

            RuleFor(x => x.CardSecurityCode)
                .Matches(@"^\d{3}$")
                .WithMessage("Código de verificação (CVV) do cartão inválido");
        }
    }
}