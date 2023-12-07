using FluentAssertions;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Infrastructure.Persistence.Mappers;
using SimplePSP.Tests.Common.Builders.Domain;
using SimplePSP.Tests.Common.Builders.Infrastructure;

namespace SimplePSP.Infrastructure.Persistence.UnitTests.Mappers
{
    public class PayableMapperTests
    {
        [Test]
        public void ToDTO_WhenDomainObjectIsReceived_ShouldMapToDTOCorrectly()
        {
            var payable = new PayableBuilder().Generate();

            var dto = PayableMapper.ToDTO(payable);

            dto.Id.Should().Be(payable.Id);
            dto.Status.Should().Be(payable.Status.ToString());
            dto.PaymentDate.Should().Be(payable.PaymentDate);
            dto.Value.Should().Be(payable.Value);
            dto.Transaction.Should().BeEquivalentTo(TransactionMapper.ToDTO(payable.Transaction));
        }

        [Test]
        public void ToDomain_WhenDTOObjectIsReceived_ShouldMapToDomainCorrectly()
        {
            var dto = new PayableDTOBuilder().Generate();

            var payable = PayableMapper.ToDomain(dto);

            payable.Id.Should().Be(dto.Id);
            payable.Status.Should().Be(Enum.Parse<PayableStatus>(dto.Status));
            payable.PaymentDate.Should().Be(dto.PaymentDate);
            payable.Value.Should().Be(dto.Value);
            payable.Transaction.Should().BeEquivalentTo(TransactionMapper.ToDomain(dto.Transaction));
        }
    }
}