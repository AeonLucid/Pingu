using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pingu.Web.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly Random Random = new Random();

        private const string Salt = "liu7gey497t34y#YEYWye7h9y3%@&YQAyutshd";

        public void OnGet()
        {
            var username = $"Player{Random.Next(10000, 99999)}";
            string hash;

            using (var hasher = new MD5CryptoServiceProvider())
            {
                var bytes = hasher.ComputeHash(Encoding.ASCII.GetBytes($"{Salt}{username}"));

                hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }

            ViewData["Username"] = username;
            ViewData["Hash"] = hash;
        }
    }
}