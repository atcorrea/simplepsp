using Bogus;
using FluentAssertions;
using SimplePSP.API.Models.Request;
using SimplePSP.API.Validators;

namespace SimplePSP.API.UnitTests
{
    public class CreateTransactionRequestValidatorTests
    {
        private readonly Faker faker = new();

        [Test]
        public void Validate_WhenStoreIdIsEmpty_ShouldReturnFalse()
        {
            var request = new CreateTransactionRequest()
            {
                Value = 42,
                CardNumber = "1234-5678-9876-5432",
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "O campo StoreId deve ser informado");
        }

        [TestCase(0)]
        [TestCase(-45.32)]
        public void Validate_WhenValueIsLessThanZero_ShouldReturnFalse(decimal value)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = value,
                CardNumber = "1234-5678-9876-5432",
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "Valor da transação deve maior que zero");
        }

        [TestCase(1)]
        [TestCase(42.32)]
        public void Validate_WhenValueIsGreaterThanZero_ShouldReturnTrue(decimal value)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = value,
                CardNumber = "1234567898765432",
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [TestCase("1234-5678-9876-5432")]
        [TestCase("1234 5678 9876 5432")]
        [TestCase("1234567898765432")]
        public void Validate_WhenCardNumberIsInCorrectFormat_ShouldReturnTrue(string cardNumber)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = cardNumber,
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [TestCase("44")]
        [TestCase("1234 5678 9876 5432 4444")]
        [TestCase("1a2b 3c4d 5e6f 7g8h")]
        [TestCase("1a2b-3c4d-5e6f-7g8h")]
        [TestCase("")]
        public void Validate_WhenCardNumberIsNotInCorrectFormat_ShouldReturnFalse(string cardNumber)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = cardNumber,
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "Número do cartão deve possuir 16 números");
        }

        [Test]
        public void Validate_WhenCardNumberNull_ShouldReturnFalse()
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "Número do cartão deve ser informado");
        }

        [TestCase(0)]
        [TestCase(9)]
        [TestCase(41)]
        [TestCase(120)]
        public void Validate_WhenCardHolderNameHasLessThan10OrMoreThan40Chars_ShouldReturnFalse(int numberOfChars)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = "1234567898765432",
                CardHolderName = faker.Random.String2(numberOfChars),
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "Nome do portador deve possuir entre 10 e 40 caracteres");
        }

        [TestCase(10)]
        [TestCase(40)]
        [TestCase(29)]
        public void Validate_WhenCardHolderNameIsBetween10And40Chars_ShouldReturnTrue(int numberOfChars)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = "1234567898765432",
                CardHolderName = faker.Random.String2(numberOfChars),
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_WhenCardValidityIsBeforeCurrentDateMonth_ShouldReturnFalse()
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = "1234567898765432",
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddMonths(-1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "A data de validade deve ser no mínimo o final do mês corrente");
        }

        [Test]
        public void Validate_WhenCardValidityIsInCurrentDateMonth_ShouldReturnTrue()
        {
            var currentDate = DateTime.Now;
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = "1234567898765432",
                CardHolderName = "NOME DO TESTE",
                CardValidity = new DateTime(currentDate.Year, currentDate.Month, 1),
                CardSecurityCode = "001"
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [TestCase("4445")]
        [TestCase("abc")]
        [TestCase("1")]
        public void Validate_WhenCardSecurityCodeLenghtIsDifferentThan3_ShouldReturnFalse(string cvv)
        {
            var request = new CreateTransactionRequest()
            {
                StoreId = "123467891011",
                Value = 42,
                CardNumber = "1234567898765432",
                CardHolderName = "NOME DO TESTE",
                CardValidity = DateTime.Now.AddYears(1),
                CardSecurityCode = cvv
            };
            var validator = new CreateTransactionRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "Código de verificação (CVV) do cartão inválido");
        }
    }
}