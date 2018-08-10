using System.Collections.Generic;
using Convert.Express.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Convert.Express.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess _db;

        public IndexModel(DataAccess db)
        {
            _db = db;
        }

        public void OnGet(string q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                var converter = new Converter();

                InputConvertExpress.Querystring = q;
                Result = converter.Convert(q);

            }

        }

        public InputConvertExpress InputConvertExpress => new InputConvertExpress();

        public List<Result> Result = new List<Result>();
    }
}