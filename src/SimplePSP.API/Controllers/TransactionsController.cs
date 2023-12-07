using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog.Extensions.Hosting;
using SimplePSP.API.Extensions;
using SimplePSP.API.Models.Request;
using SimplePSP.API.Models.Response;
using SimplePSP.Application.Services;
using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IValidator<CreateTransactionRequest> _validator;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IPaymentsService _paymentsService;

        public TransactionsController(IValidator<CreateTransactionRequest> validator,
                                      IPaymentsService paymentsService,
                                      ILogger<TransactionsController> logger)
        {
            _validator = validator;
            _logger = logger;
            _paymentsService = paymentsService;
        }

        /// <summary>
        /// Retorna uma lista das transações já criadas
        /// </summary>
        /// <returns>Lista com todas as transações salvas</returns>
        /// <response code="200">Retorna a lista de transações</response>
        /// <response code="500">Ocorreu um erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(typeof(IList<GetTransactionResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<GetTransactionResponse>>> GetAll()
        {
            var result = await _paymentsService.GetAllTransactionsList();

            if (!result.Success)
                return Problem(result.ErrorMessage, Request.Path, 500);

            var response = result.Value!.Select(GetTransactionResponse.CreateFromTransaction).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Salva e inicia o processamento de uma nova transação
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /Transactions
        ///     {
        ///      "storeId": "18164750000164",
        ///      "value": 42,
        ///      "description": "A purchase",
        ///      "paymentMethod": "credit_card",
        ///      "cardNumber": "1234-5678-9876-5555",
        ///      "cardHolderName": "NOME DO PORTADOR",
        ///      "cardValidity": "2023-12-05",
        ///      "cardSecurityCode": "123"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Dados da transação</param>
        /// <returns>Id da transação registrada</returns>
        /// <response code="201">Retorna a transação recém criada</response>
        /// <response code="400">Há um problema com os dados enviados</response>
        /// <response code="500">Ocorreu um erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTransactionResponseModel), 201)]
        [ProducesResponseType(typeof(ValidationProblem), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<CreateTransactionResponseModel>> Create(CreateTransactionRequest request)
        {
            var validation = _validator.Validate(request);
            if (!validation.IsValid)
            {
                _logger.LogInformation("transação inválida");

                return ValidationProblem(new ValidationProblemDetails()
                {
                    Title = "Há um problema com a transação enviada",
                    Errors = validation.GetErrorsDictionary()
                });
            }

            var transaction = new Transaction(request.StoreId,
                                                     request.Value,
                                                     request.Description,
                                                     request.PaymentMethod,
                                                     new CardNumber(request.CardNumber),
                                                     request.CardHolderName,
                                                     request.CardValidity,
                                                     request.CardSecurityCode,
                                                     DateTime.Now);

            using (_logger.GetLogScopeForTransaction(transaction))
            {
                var result = await _paymentsService.StartTransaction(transaction);

                if (!result.Success)
                    return Problem(result.ErrorMessage, Request.Path, 500);

                return Created($"{Request.Host}/{Request.Path}/{transaction.Id}", transaction.Id);
            }
        }
    }
}