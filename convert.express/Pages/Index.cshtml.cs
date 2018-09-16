using System.Collections.Generic;
using Convert.Express.Converters;
using Convert.Express.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Convert.Express.Pages
{
    public class IndexModel : PageModel
    {

        public void OnGet(string q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                var converter = new SmartConverter();
                Query= q;
                Result = converter.Convert(q);
            }
        }

        public string Query;

        public List<Result> Result = new List<Result>();
    }
}