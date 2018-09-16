using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convert.Express.Interfaces;

namespace Convert.Express.Models
{
    public class Base64Converter : IConverter
    {
        public (bool IsValid, string Output) TryConvertFrom(string input)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(input);
                return (true, System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (false, null);
            }
        }

        public (bool IsValid, string Output) TryConvertTo(string input)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                return (true, System.Convert.ToBase64String(bytes));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (false, null);
            }
        }
    }
}
