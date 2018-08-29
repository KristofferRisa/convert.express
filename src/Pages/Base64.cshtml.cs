using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Convert.Express.Pages
{
    public class Base64Model : PageModel
    {
        public void OnGet()
        {
            var type = Request.Query["type"];
            var content = Request.Query["content"];
            var encoding = Request.Query["encoding"];

            switch (encoding)
            {
                case "encode":
                    ViewData["Encoded"] = Base64Encode(content);
                    ViewData["Encode"] = content;
                    break;
                case "decode":
                    ViewData["Decoded"] = Base64Decode(content);
                    ViewData["Decode"] = content;
                    break;
            }

        }

        public void OnPost(string encode, string decode)
        {
            if (!string.IsNullOrEmpty(encode))
            {
                ViewData["Encoded"] = Base64Encode(encode);
                ViewData["Encode"] = encode;
            }

            if (string.IsNullOrEmpty(decode)) return;
            ViewData["Decoded"] = Base64Decode(decode);
            ViewData["Decode"] = decode;

        }

        #region helper methods
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

    }

}