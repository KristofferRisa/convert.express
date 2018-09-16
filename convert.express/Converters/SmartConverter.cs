using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Convert.Express.Models;

namespace Convert.Express.Converters
{
    public class SmartConverter
    {
        public List<Result> Convert(string input)
        {
            var _data = new List<Result>();
            try
            {
                var converter = new CurrencyConverter();
                _data.Add(new Result()
                {
                    Header = "Currency"
                    ,
                    Description = $"{converter.Convert(input).ToString(CultureInfo.InvariantCulture)}"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (input.Length > 6 
                && input.Substring(0, 6).ToUpper().Trim() == "BASE64")
            {
                var baseConverter = new Base64Converter();
                _data.Add(new Result()
                {
                    Header = "Base64",
                    Description = baseConverter.TryConvertFrom(input.Substring(7, input.Length - 7)).Output
                });
                _data.Add(new Result()
                {
                    Header = "Base64",
                    Description = baseConverter.TryConvertTo(input.Substring(7, input.Length - 7)).Output
                });

                
            }
            else if (input.Length > 3
                && (input.Substring(input.Length - 2, 2) == "=="
                || input.Substring(input.Length - 1, 1) == "="))
            {
                var baseConverter = new Base64Converter();
                _data.Add(new Result()
                {
                    Header = "Base64",
                    Description = baseConverter.TryConvertFrom(input).Output
                });
                
            }

            if (!_data.Any())
                _data.Add(new Result()
                {
                    Header = ":(",
                    Description = "Try something else.."
                });

            return _data;
        }
    }
}
