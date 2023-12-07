using FluentAssertions;
using SimplePSP.Domain.TransactionAggregate;
using SimplePSP.Infrastructure.Persistence.Mappers;
using SimplePSP.Tests.Common.Builders.Domain;
using SimplePSP.Tests.Common.Builders.Infrastructure;

namespace SimplePSP.Infrastructure.Persistence.UnitTests.Mappers
{
    public class TransactionMapperTests
    {
        [Test]
        public void ToDTO_WhenDomainObjectIsReceived_ShouldMapToDTOCorrectly()
        {
            var transaction = new TransactionBuilder().Generate();

            var dto = TransactionMapper.ToDTO(transaction);

            dto.Id.Should().Be(transaction.Id);
            dto.StoreId.Should().Be(transaction.StoreId);
            dto.Value.Should().Be(transaction.Value);
            dto.Description.Should().Be(transaction.Description);
            dto.PaymentMethod.Should().Be(transaction.PaymentMethod.ToString());
            dto.CardHolderName.Should().Be(transaction.CardHolderName);
            dto.CardValidity.Should().Be(transaction.CardValidity);
            dto.CardSecurityCode.Should().Be(transaction.CardSecurityCode);
            dto.CreateDate.Should().BeCloseTo(transaction.CreateDate, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void ToDTO_WhenCreditCardNumberIsReceived_SaveMapOnlyLast4Digits()
        {
            var transaction = new TransactionBuilder().Generate();

            var dto = TransactionMapper.ToDTO(transaction);

            dto.CardNumber.Should().Be(transaction.CardNumber.Last4Digits);
        }

        [Test]
        public void ToDomain_WhenDTOObjectIsReceived_ShouldMapToDomainCorrectly()
        {
            var dto = new TransactionDTOBuilder().Generate();

            var transaction = TransactionMapper.ToDomain(dto);

            transaction.Id.Should().Be(dto.Id);
            transaction.StoreId.Should().Be(dto.StoreId);
            transaction.Value.Should().Be(dto.Value);
            transaction.Description.Should().Be(dto.Description);
            transaction.PaymentMethod.Should().Be(Enum.Parse<PaymentMethod>(dto.PaymentMethod));
            transaction.CardNumber.Should().BeEquivalentTo(new CardNumber(dto.CardNumber.PadLeft(16, '*')));
            transaction.CardHolderName.Should().Be(dto.CardHolderName);
            transaction.CardValidity.Should().Be(dto.CardValidity);
            transaction.CardSecurityCode.Should().Be(dto.CardSecurityCode);
        }
    }
}