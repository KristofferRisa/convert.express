using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Convert.Express.Models.CC
{
    internal class Client
    {

        private readonly Dictionary<string, Currency> _cache;

        internal Client()
        {
            if (_cache == null)
            {
                _cache = new Dictionary<string, Currency>();
            }
        }

        internal Currency GetCurrency(string from, string to)
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

                var currency = JsonConvert.DeserializeObject<global::Convert.Express.Models.CC.Currency>(responsestring);

                _cache.Add(from+to, currency);
                return currency;
            }
        }
    }
}
