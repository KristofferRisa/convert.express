using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Convert.Express.Models.CC
{
    public class Converter
    {
        private readonly List<RegExSpilt> _regexSpilters;
        private readonly Client _client;
        private string _localIsoSymbol = RegionInfo.CurrentRegion.ISOCurrencySymbol;

        public void SetLocalIso(string localIso) => _localIsoSymbol = localIso;

        public Converter()
        {
            if(_regexSpilters == null)
                _regexSpilters = new List<RegExSpilt>()
                {
                    new RegExSpilt(){
                        Regex =  @"^(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})$", //10 usd
                        FirstSplit = 1,
                        SecondSplit = null
                    },
                    new RegExSpilt()
                    {
                        Regex =  @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([i][n])\s([A-Za-z]{3})", // 10 usd in nok
                        FirstSplit = 1,
                        SecondSplit = 3
                    },
                    new RegExSpilt()
                    {
                        Regex =  @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([t][o])\s([A-Za-z]{3})", // 10 usd to nok
                        FirstSplit = 1,
                        SecondSplit = 3
                    },
                    new RegExSpilt()
                    {
                        Regex =  @"(\d+(\.\d{1,2})?)?\s([A-Za-z]{3})\s([a][s])\s([A-Za-z]{3})", // 10 usd as nok
                        FirstSplit = 1,
                        SecondSplit = 3
                    }
                };
            _client = new Client();
        }

        public Converter(List<RegExSpilt> regexSpilter)
        {
            _client = new Client();
            _regexSpilters = regexSpilter;
        }

        public string CurrencyConvert(string query)
        {
            foreach (var regex in _regexSpilters)
            {
                if (Regex.IsMatch(query, regex.Regex))
                {
                    var from = query.Split(' ')[regex.FirstSplit].ToUpper();
                    var to = regex.SecondSplit != null ? query.Split(' ')[regex.SecondSplit ?? 0].ToUpper() : _localIsoSymbol;
                    var amount = System.Convert.ToDecimal(query.Split(' ')[regex.AmountSplit]);
                    return $"{GetCurrencyAmount(amount,from,to)} {to.ToUpper()}";
                }
            }
            return null;
        }

        public string CurrencyConvert(decimal amount,string from, string to)
        {
            return $"{GetCurrencyAmount(amount,from,to)} {to.ToUpper()}";
        }

        public decimal GetCurrencyAmount(decimal amount, string from, string to)
        {
            return Math.Round(amount * _client.GetCurrency(from, to).GetRate(to), 2);
        }
    }
}
