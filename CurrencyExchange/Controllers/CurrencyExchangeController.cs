using System;
using CurrencyExchange.Models;
using CurrencyExchange.Service;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [ApiController]
    [Route("[currencyExchange/api]")]
    public class CurrencyExchangeController : ControllerBase
	{
		private ILogger<CurrencyExchangeController> _logger;


		private CurrencyExchangeService _service;

        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger, CurrencyExchangeService service)
		{
			_logger = logger;
			_service = service;
			
        }

		[HttpGet]
		[Route("/{fromCurrency}/{toCurrency}/{amount}")]
		public async Task<ActionResult<CurrencyExchangeModel>> getCurrencyExchangeRate(string fromCurrency, string toCurrency, int amount)
		{
			_logger.LogInformation("Get currency exchange started");
            CurrencyExchangeModel response = await _service.getCurrencyExchange(fromCurrency, toCurrency, amount);

			if(response == null)
			{
				_logger.LogInformation("getCurrencyExchange executed with null response");
				return null;
			}
            _logger.LogInformation("getCurrencyExchange executed successfully");
            return Ok(response);
		}


	}
}

