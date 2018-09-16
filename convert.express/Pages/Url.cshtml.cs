using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Convert.Express.Pages
{
    public class UrlModel : PageModel
    {
        public void OnGet()
        {
            var type = Request.Query["type"];
            var content = Request.Query["content"];

            if (type == "EncodeUrl")
            {
                ViewData["Encoded"] = WebUtility.UrlEncode(content);
                ViewData["Encode"] = content;
            }
            else if (type == "DecodeUrl")
            {
                ViewData["Decoded"] = WebUtility.UrlDecode(content);
                ViewData["Decode"] = content;
            }
        }

        public void OnPost(string encode, string decode)
        {
            if (!string.IsNullOrEmpty(encode))
            {
                ViewData["Encoded"] = WebUtility.UrlEncode(encode);
                ViewData["Encode"] = encode;
            }

            if (!string.IsNullOrEmpty(decode))
            {
                ViewData["Decoded"] = WebUtility.UrlDecode(decode);
                ViewData["Decode"] = decode;
            }
        }
    }
}