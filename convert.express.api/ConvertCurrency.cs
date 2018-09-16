
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.Globalization;

namespace Convert.Express.Api
{
    public static class ConvertCurrency
    {
        [FunctionName("ConvertCurrency")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
        
            string input = req.Query["convert"];
            
            string _localIsoSymbol = RegionInfo.CurrentRegion.ISOCurrencySymbol;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            input = input ?? data?.input;
            
            //From and to converstion
            foreach (var regex in GetregExs())
            {
                if (Regex.IsMatch(input, regex))
                {
                    var input_split = input.Split(' ');
                    var from = input_split[1].ToUpper();
                    var to = _localIsoSymbol;
                    if(input_split.Length > 3 )
                    {
                        to = input_split[3].ToUpper();
                    }
                    else if(input_split.Length > 2)
                    {
                        to = input_split[2].ToUpper();
                    }
                    var amount = System.Convert.ToDecimal(input_split[0]);
                    return (ActionResult)new OkObjectResult($"{GetCurrencyAmount(amount,from,to)} {to.ToUpper()}");
                }
            }

            return new BadRequestObjectResult("Please pass a name on the query string or in the request body");;
        }

        public static decimal GetCurrencyAmount(decimal amount, string from, string to)
        {
            var _client = new FixerApiClient();
            return Math.Round(amount * _client.GetCurrency(from, to).GetRate(to), 2);
        }

        private static List<string> GetregExs() => new List<string>(){
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([i][n])\s([A-Za-z]{3})", // 10 usd in nok
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([t][o])\s([A-Za-z]{3})", // 10 usd to nok
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([a][s])\s([A-Za-z]{3})", // 10 usd as nok    
                @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([A-Za-z]{3})$", //10 usd nok
                @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})$", // 10 usd
            };
    }
    internal class FixerApiClient
    {
        private readonly static Dictionary<string, Currency> _cache = new Dictionary<string, Currency>();

        public Currency GetCurrency(string from, string to)
        {
            var apikey = Environment.GetEnvironmentVariable("fixer_apikey") ?? "d3a1aabce92c079871e0a8ec02b3b824";
            var url = $"https://data.fixer.io/api/latest?base={from}&symbols={to}&access_key={apikey}";

            if (_cache.ContainsKey(from+to))
            {
                if (System.Convert.ToDateTime(_cache[from+to].date) == DateTime.Today)
                {
                    return _cache[from+to];
                }
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponseAsync().Result;
            using (new StreamReader(response.GetResponseStream()))
            {
                var responsestring = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var currency = JsonConvert.DeserializeObject<Currency>(responsestring);

                _cache.Add(from+to, currency);
                return currency;
            }
        }
    }
    public class Currency
    {
        [JsonProperty]
        public string @base { get; set; }
        [JsonProperty]
        public string date { get; set; }
        [JsonProperty]
        public Rates rates { get; set; }

        public decimal GetRate(string currency)
        {
            return System.Convert.ToDecimal(this.rates.GetType().GetProperty(currency).GetValue(rates, null));
        }
    }
    public class Rates
    {
        public float EUR { get; set; }
        public float AUD { get; set; }
        public float BGN { get; set; }
        public float BRL { get; set; }
        public float CAD { get; set; }
        public float CHF { get; set; }
        public float CNY { get; set; }
        public float CZK { get; set; }
        public float DKK { get; set; }
        public float GBP { get; set; }
        public float HKD { get; set; }
        public float HRK { get; set; }
        public float HUF { get; set; }
        public float IDR { get; set; }
        public float ILS { get; set; }
        public float INR { get; set; }
        public float JPY { get; set; }
        public float KRW { get; set; }
        public float MXN { get; set; }
        public float MYR { get; set; }
        public float NOK { get; set; }
        public float NZD { get; set; }
        public float PHP { get; set; }
        public float PLN { get; set; }
        public float RON { get; set; }
        public float RUB { get; set; }
        public float SEK { get; set; }
        public float SGD { get; set; }
        public float THB { get; set; }
        public float TRY { get; set; }
        public float USD { get; set; }
        public float ZAR { get; set; }
    }
    public enum RateList
    {
        EUR,
        AUD,
        BGN,
        BRL,
        CAD,
        CHF,
        CNY,
        CZK,
        DKK,
        GBP,
        HKD,
        HRK,
        HUF,
        IDR,
        ILS,
        INR,
        JPY,
        KRW,
        MXN,
        MYR,
        NOK,
        NZD,
        PHP,
        PLN,
        RON,
        RUB,
        SEK,
        SGD,
        THB,
        TRY,
        USD,
        ZAR,
    }

}
