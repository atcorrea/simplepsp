using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SimplePSP.API.Models.Request;
using SimplePSP.API.Models.Response;
using SimplePSP.Domain.Repositories;
using SimplePSP.Tests.Common.Builders.API;
using SimplePSP.Tests.Common.Builders.Domain;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimplePSP.API.IntegrationTests.Controllers
{
    public class TransactionsControllerTests : TestBase
    {
        private const string PATH = "transactions";
        private JsonSerializerOptions jsonOptions;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, allowIntegerValues: false));
        }

        [Test]
        public async Task Create_WhenTransactionIsValid_ShouldReturn201Created()
        {
            var request = new CreateTransactionRequestBuilder().Generate();

            var httpResponse = await Client.PostAsJsonAsync(PATH, request, jsonOptions);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task Create_WhenTransactionIsNotValid_ShouldReturnBadRequest()
        {
            var request = new CreateTransactionRequestBuilder()
                .WithCardNumber("NO CARD")
                .Generate();

            var httpResponse = await Client.PostAsJsonAsync(PATH, request, jsonOptions);

            var resonseContent = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ValidationProblemDetails>(resonseContent, jsonOptions)!;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Title.Should().Be("Há um problema com a transação enviada");
            response.Errors.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string[]>(nameof(CreateTransactionRequest.CardNumber), ["Número do cartão deve possuir 16 números"]));
        }

        [Test]
        public async Task GetAll_WhenCalled_ReturnAllSavedTransactions()
        {
            var existingTransactions = new TransactionBuilder().Generate(4);
            var repo = Services.GetRequiredService<ITransactionRepository>();
            foreach (var transaction in existingTransactions)
                await repo.Create(transaction);

            var httpResponse = await Client.GetAsync(PATH);
            var resonseContent = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<List<GetTransactionResponse>>(resonseContent)!;

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().HaveCount(existingTransactions.Count);
        }
    }
}