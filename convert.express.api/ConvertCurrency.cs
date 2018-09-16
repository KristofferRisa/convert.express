
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
                    var convertedAmount = FixerApiClient.GetCurrency(from, to,amount.ToString(),log);
                    return (ActionResult)new OkObjectResult($"{convertedAmount} {to}");
                }
            }

            return new BadRequestObjectResult("Please pass a name on the query string or in the request body");;
        }

        private static List<string> GetregExs() => new List<string>(){
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([Ii][nn])\s([A-Za-z]{3})", // 10 usd in nok
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([tt][Oo])\s([A-Za-z]{3})", // 10 usd to nok
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([Aa][Ss])\s([A-Za-z]{3})", // 10 usd as nok    
                @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([A-Za-z]{3})$", //10 usd nok
                @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})$", // 10 usd
            };
    }
    public static class FixerApiClient
    {
        private readonly static Dictionary<string, Currency> _cache = new Dictionary<string, Currency>();

        public static double GetCurrency(string from, string to, string amount,ILogger log)
        {
            var apikey = Environment.GetEnvironmentVariable("fixer_apikey") ?? "d3a1aabce92c079871e0a8ec02b3b824";
            var url = $"https://data.fixer.io/api/convert?from={from}&to={to}&amount={amount}&access_key={apikey}";

            if (_cache.ContainsKey(from+to+amount))
            {
                if (System.Convert.ToDateTime(_cache[from+to+amount].Date) == DateTime.Today)
                {
                    log.LogInformation("Found query in cache, returning cached data.");
                    return _cache[from+to+amount].Result;
                }
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponseAsync().Result;
            using (new StreamReader(response.GetResponseStream()))
            {
                var responsestring = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var currency = JsonConvert.DeserializeObject<Currency>(responsestring);

                _cache.Add(from+to+amount, currency);
                return currency.Result;
            }
        }
    }
    public class Currency
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("historical")]
        public string Historical { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("result")]
        public double Result { get; set; }
        
    }
    public partial class Info
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }
    }
    public partial class Query
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

}
