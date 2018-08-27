using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Convert.Express.Interfaces;

namespace Convert.Express.Models
{
    public class LiterConverter : IConverter
    {
        private string LiterRegEx => @"^[\d\s]+ +[lL]";

        public (bool IsValid, string Output) TryConvertFrom(string input)
        {
            if(Regex.Match(input,LiterRegEx).Success)
            {
                try
                {
                    decimal amount = System.Convert.ToDecimal(input);
                    return (true, $"{amount / 100} dL");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return (false, null);
        }

        public (bool IsValid, string Output) TryConvertTo(string input)
        {
            if (Regex.Match(input, LiterRegEx).Success)
            {
                try
                {
                    decimal amount = System.Convert.ToDecimal(input);
                    return (true, $"{amount * 100} dL");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return (false, null);
        }
    }
}
