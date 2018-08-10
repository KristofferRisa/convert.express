using Newtonsoft.Json;

namespace Convert.Express.Models.CC
{
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
    
}
