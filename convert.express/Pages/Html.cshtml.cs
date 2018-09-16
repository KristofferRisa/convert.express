using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Convert.Express.Pages
{
    public class HtmlModel : PageModel
    {
        public void OnGet()
        {
            var type = Request.Query["type"];
            var content = Request.Query["content"];

            if (type == "EncodeHtml")
            {
                ViewData["Encoded"] = WebUtility.HtmlEncode(content);
                ViewData["Encode"] = content;
            }
            else if (type == "DecodeHtml")
            {
                ViewData["Decoded"] = WebUtility.HtmlDecode(content);
                ViewData["Decode"] = content;
            }
        }
        [ValidateAntiForgeryToken]
        public void OnPost(string encode, string decode)
        {
            if (!string.IsNullOrEmpty(encode))
            {
                ViewData["Encoded"] = WebUtility.HtmlEncode(encode);
                ViewData["Encode"] = encode;
            }

            if (!string.IsNullOrEmpty(decode))
            {
                ViewData["Decoded"] = WebUtility.HtmlDecode(decode);
                ViewData["Decode"] = decode;
            }
        }
    }
}