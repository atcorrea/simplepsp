using Microsoft.AspNetCore.Mvc;
using SimplePSP.API.Extensions;
using SimplePSP.API.Models.Response;
using SimplePSP.Application.Services;

namespace SimplePSP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly IStoreBalanceService _storeBalanceService;
        private readonly ILogger<StoresController> _logger;

        public StoresController(IStoreBalanceService storeBalanceService,
                                ILogger<StoresController> logger)
        {
            _storeBalanceService = storeBalanceService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna o saldo com tudo que o cliente já recebeu
        /// </summary>
        /// <param name="storeId">Identificador do cliente</param>
        /// <returns>Saldo disponível do cliente</returns>
        /// <response code="200">Retorna o saldo</response>
        /// <response code="500">Ocorreu um erro no processamento da requisição</response>
        [ProducesResponseType(typeof(BalanceResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        [HttpGet("{storeId}/balances/available")]
        public async Task<IActionResult> GetAvailableBalance(string storeId)
        {
            using (_logger.GetLogScopeWithProperty("storeId", storeId))
            {
                var result = await _storeBalanceService.GetAvailableBalance(storeId);
                if (!result.Success)
                    return Problem(result.ErrorMessage, Request.Path, 500);

                return Ok(new BalanceResponse(result.Value));
            }
        }

        /// <summary>
        /// Retorna o saldo com tudo que o cliente tem a receber
        /// </summary>
        /// <param name="storeId">Identificador do cliente</param>
        /// <returns>Saldo disponível do cliente</returns>
        /// <response code="200">Retorna o saldo</response>
        /// <response code="500">Ocorreu um erro no processamento da requisição</response>
        [ProducesResponseType(typeof(BalanceResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        [HttpGet("{storeId}/balances/incoming")]
        public async Task<IActionResult> GetIncomingBalance(string storeId)
        {
            using (_logger.GetLogScopeWithProperty("storeId", storeId))
            {
                var result = await _storeBalanceService.GetIncomingBalance(storeId);

                if (!result.Success)
                    return Problem(result.ErrorMessage, Request.Path, 500);

                return Ok(new BalanceResponse(result.Value));
            }
        }
    }
}