using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CurrencyExchange.Models
{
	public class CurrencyExchangeExternalModel
	{
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("base")]
        public string? FromCurrency { get; set; }

        [JsonProperty("date")]
        [DataType(DataType.Date)]
        public DateTime ConvertDate { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, string>? ConvertionRates { get; set; }
    }
}

