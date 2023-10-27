using System;
using CurrencyExchange.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CurrencyExchange.Service
{
    public class CurrencyExchangeService
    {
        private ILogger<CurrencyExchangeService> _logger;
        private string baseApiUrl = "http://data.fixer.io/api";
        private string latestRateUrl = "/latest?access_key=63225fef82d195e08826824c54afd70e&base={0}&symbols={1}";
        public string withDateRateUrl = "/{0}?access_key=63225fef82d195e08826824c54afd70e&base={1}&symbols={2}";

        public CurrencyExchangeService(ILogger<CurrencyExchangeService> logger)
        {
            _logger = logger;

        }

        public async Task<CurrencyExchangeModel> getCurrencyExchange(string fromCurrency, string toCurrency, int amount, DateTime ? date = null)
        {
            try
            {
                _logger.LogInformation("Started getting Currency Exchange in Service Class");
                string externalApiUrl = string.Empty;

                if (date.HasValue)
                {
                    externalApiUrl = string.Format(string.Concat(baseApiUrl, withDateRateUrl), date.Value.ToString("yyyy-MM-dd"), fromCurrency, toCurrency);

                }
                else
                {
                    externalApiUrl = string.Format(string.Concat(baseApiUrl, latestRateUrl), fromCurrency, toCurrency);
                }
                _logger.LogInformation("external url :{}", externalApiUrl);

                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(externalApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to get exchange reate from external API");
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                CurrencyExchangeExternalModel exchangeRateResponse = JsonConvert.DeserializeObject<CurrencyExchangeExternalModel>(responseBody);

                CurrencyExchangeModel actualResponse = new();
                if (exchangeRateResponse.Success)
                {
                    string? exchangeValue = exchangeRateResponse.ConvertionRates?.Values?.FirstOrDefault();
                    double convertedRate = double.Parse(exchangeValue) * amount;
                    actualResponse.Rate = convertedRate;
                }
                else
                {
                    actualResponse.ErrorMessage = "External call to convert rate is success but failure in the actual response";
                }

                return actualResponse;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured in getCurrencyExchange ", e);
                throw new Exception(e.Message);

            }
        }
    }
}

