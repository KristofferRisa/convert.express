using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Convert.Express.Interfaces;
using Newtonsoft.Json;

namespace Convert.Express.Models
{
    internal class FixerApiClient : ICurrencyApiClient
    {
        private readonly Dictionary<string, Currency> _cache;

        internal FixerApiClient()
        {
            if (_cache == null)
            {
                _cache = new Dictionary<string, Currency>();
            }
        }

        public Currency GetCurrency(string from, string to)
        {
            var apikey = Environment.GetEnvironmentVariable("fixer_apikey") ?? "";
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

                var currency = JsonConvert.DeserializeObject<global::Convert.Express.Models.Currency>(responsestring);

                _cache.Add(from+to, currency);
                return currency;
            }
        }
    }
}
