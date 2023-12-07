using FluentAssertions;
using SimplePSP.API.Models.Response;
using SimplePSP.Domain.PayableAggregate;
using SimplePSP.Infrastructure.Persistence.Mappers;
using SimplePSP.Tests.Common.Builders.Domain;
using System.Net;
using System.Text.Json;

namespace SimplePSP.API.IntegrationTests.Controllers
{
    public class StoresControllerTests : TestBase
    {
        private const string PATH = "stores";

        [Test]
        public async Task GetAvailableBalance_WhenStoreExists_ShouldReturn200()
        {
            const string storeId = "111111111111111";
            var httpResponse = await Client.GetAsync($"{PATH}/{storeId}/balances/available");

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAvailableBalance_WhenStoreExists_ShouldResponseWithBalance()
        {
            const string storeId = "111111111111111";
            var paidPayablesForStore = new PayableBuilder()
                                            .WithStatus(PayableStatus.Paid)
                                            .WithTransactionBuilder(new TransactionBuilder()
                                                                        .WithStoreId(storeId))
                                            .Generate(4)
                                            .Select(PayableMapper.ToDTO);
            DbContext.Payables.AddRange(paidPayablesForStore);
            await DbContext.SaveChangesAsync();

            var httpResponse = await Client.GetAsync($"{PATH}/{storeId}/balances/available");
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<BalanceResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

            var expectedBalance = paidPayablesForStore.Sum(x => x.Value);
            response.Balance.Should().Be(expectedBalance);
        }

        [Test]
        public async Task GetIncomingBalance_WhenStoreExists_ShouldReturn200()
        {
            const string storeId = "111111111111111";
            var httpResponse = await Client.GetAsync($"{PATH}/{storeId}/balances/incoming");

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetIncomingBalance_WhenStoreExists_ShouldResponseWithBalance()
        {
            const string storeId = "111111111111111";
            var incomingPayablesForStore = new PayableBuilder()
                                            .WithStatus(PayableStatus.WaitingFunds)
                                            .WithTransactionBuilder(new TransactionBuilder()
                                                                        .WithStoreId(storeId))
                                            .Generate(4)
                                            .Select(PayableMapper.ToDTO);
            DbContext.Payables.AddRange(incomingPayablesForStore);
            await DbContext.SaveChangesAsync();

            var httpResponse = await Client.GetAsync($"{PATH}/{storeId}/balances/incoming");
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<BalanceResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

            var expectedBalance = incomingPayablesForStore.Sum(x => x.Value);
            response.Balance.Should().Be(expectedBalance);
        }
    }
}