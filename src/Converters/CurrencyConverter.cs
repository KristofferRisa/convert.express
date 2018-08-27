using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Convert.Express.Models
{
    public class CurrencyConverter
    {
        private FixerApiClient _client => new FixerApiClient();

        private string _localIsoSymbol = RegionInfo.CurrentRegion.ISOCurrencySymbol;

        public void SetLocalIso(string localIso) => _localIsoSymbol = localIso;
                
        public string Convert(string input)
        {
            var regExs = new List<string>(){
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([i][n])\s([A-Za-z]{3})", // 10 usd in nok
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([t][o])\s([A-Za-z]{3})", // 10 usd to nok
                @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([a][s])\s([A-Za-z]{3})", // 10 usd as nok    
            };
            
            //From and to converstion
            foreach (var regex in regExs)
            {
                if (Regex.IsMatch(input, regex))
                {
                    var from = input.Split(' ')[1].ToUpper();
                    var to = input.Split(' ')[3].ToUpper();
                    var amount = System.Convert.ToDecimal(input.Split(' ')[0]);
                    return $"{GetCurrencyAmount(amount,from,to)} {to.ToUpper()}";
                }
            }

            //From and to without spesification
            if (Regex.IsMatch(input, @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([A-Za-z]{3})$")) //10 usd nok
            {
                var from = input.Split(' ')[1].ToUpper();
                var to = input.Split(' ')[2].ToUpper();
                var amount = System.Convert.ToDecimal(input.Split(' ')[0]);
                return $"{GetCurrencyAmount(amount, from, to)} {to.ToUpper()}";
            }

            //Singel from convert
            if (Regex.IsMatch(input, @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})$")) //10 usd
            {
                var from = input.Split(' ')[1].ToUpper();
                var to = _localIsoSymbol;
                var amount = System.Convert.ToDecimal(input.Split(' ')[0]);
                return $"{GetCurrencyAmount(amount, from, to)} {to.ToUpper()}";
            }

            return null;
        }

        public string Convert(decimal amount,string from, string to)
        {
            return $"{GetCurrencyAmount(amount,from,to)} {to.ToUpper()}";
        }

        public decimal GetCurrencyAmount(decimal amount, string from, string to)
        {
            return Math.Round(amount * _client.GetCurrency(from, to).GetRate(to), 2);
        }
    }
}
