using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Convert.Express.Models
{
    public class Converter
    {
        public Converter()
        {
            _data = new List<Result>();
        }

        public List<Result> Convert(string q)
        {
            try
            {
                var converter = new Models.CC.Converter();
                _data.Add(new Result()
                {
                    Header = "Currency"
                    , Description =  $"{converter.CurrencyConvert(q).ToString(CultureInfo.InvariantCulture)}" 
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if(Regex.Match(q,LiterRegEx).Success)
                ConvertLiter(q.Substring(0,q.Length-1).Trim());


            if (q.Length > 6 && q.Substring(0, 7).ToUpper() == "BASE64 ")
            {
                //TODO Verify this
                ConvertFromBase64(q.Substring(7, q.Length - 7));
                ConvertToBase64(q.Substring(7, q.Length - 7));
            }
            else if (q.Length > 6 && q.Substring(0, 6).ToUpper() == "BASE64")
            {
                ConvertFromBase64(q.Substring(6, q.Length - 6));
                ConvertToBase64(q.Substring(6, q.Length - 6));
            }
            else if(q.Length > 3 
                && (q.Substring(q.Length - 2, 2) == "=="
                || q.Substring(q.Length -1 ,1) == "="))
                ConvertFromBase64(q);

            if(!_data.Any())
                _data.Add(new Result()
                {
                    Header = ":(",
                    Description  = "Try something else.."
                });
            
            return _data;
        }

        private string LiterRegEx => @"^[\d\s]+ +[lL]";

        private readonly List<Result> _data;

        private void ConvertLiter(string q)
        {
            try{
                decimal amount = System.Convert.ToDecimal(q);
                _data.Add(new Result(){
                    Header = "From liter to desiliter"
                    ,Description = $"{amount * 100} dL"
                });
                _data.Add(new Result(){
                    Header = "From liter to milliliter"
                    ,Description = $"{amount * 1000} mL"
                });
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private void ConvertFromBase64(string q)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(q);
                _data.Add(new Result()
                {
                    Header = "Decoded BASE64",
                    Description = System.Text.Encoding.UTF8.GetString(base64EncodedBytes)//Encoding.UTF8.GetString(q)
            });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ConvertToBase64(string q)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(q);
                _data.Add(new Result()
                {
                    Header = "Encoded BASE64",
                    Description = System.Convert.ToBase64String(bytes)
                    
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
