using Convert.Express.Models;

namespace Convert.Express.Interfaces
{
    public interface ICurrencyApiClient
    {
        Currency GetCurrency(string from, string to);
    }
}