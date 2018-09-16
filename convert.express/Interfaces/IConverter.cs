using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convert.Express.Interfaces
{
    public interface IConverter
    {
        (bool IsValid, string Output) TryConvertTo(string input);

        (bool IsValid, string Output) TryConvertFrom(string input);
    }
}
