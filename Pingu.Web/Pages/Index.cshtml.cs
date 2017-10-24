using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Pingu.Web.Config;

namespace Pingu.Web.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly Random Random = new Random();
        
        private readonly PinguConfig _config;

        public IndexModel(IOptions<PinguConfig> optionsPingu)
        {
            _config = optionsPingu.Value;
        }

        public void OnGet()
        {
            var username = $"Player{Random.Next(10000, 99999)}";
            string hash;

            using (var hasher = new MD5CryptoServiceProvider())
            {
                var bytes = hasher.ComputeHash(Encoding.ASCII.GetBytes($"{_config.Salt}{username}"));

                hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }

            ViewData["Username"] = username;
            ViewData["Hash"] = hash;
            ViewData["Ip"] = _config.Ip;
            ViewData["Port"] = _config.Port;
        }
    }
}